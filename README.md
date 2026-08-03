# Sistema de Análisis de Ventas — Proceso ETL (.NET 8)

Proyecto académico (ITLA — Electiva 1) que implementa un **proceso ETL** (Extracción, Transformación y Carga) sobre un Data Warehouse en SQL Server, usando **.NET 8**, **Clean Architecture** y principios **SOLID**.

El proceso extrae datos de **3 fuentes distintas** (archivos CSV, base de datos relacional y API REST), los persiste en un **staging**, los transforma y los carga en un **DWH con esquema en estrella** (dimensiones + tabla de hechos).

---

## Arquitectura

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        AnalisisVentas.WkService (.NET 8)                    │
│                        Worker Service — orquesta el pipeline ETL            │
│                                                                             │
│  ┌───────────────┐   ┌───────────────┐   ┌───────────────┐                  │
│  │  CsvExtractor │   │DatabaseExtractor│ │  ApiExtractor │   IExtractor<T>  │
│  └───────┬───────┘   └───────┬───────┘   └───────┬───────┘                  │
│          │  CsvHelper        │  ADO.NET          │  IHttpClientFactory      │
│  ┌───────▼───────────────────▼───────────────────▼───────┐                  │
│  │                STAGING (archivos JSON)                │                  │
│  └───────────────────────────┬───────────────────────────┘                  │
│                              ▼                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐   │
│  │  TransformService + Escritores DWH (SCD Tipo 1 / anti-duplicado)     │   │
│  └──────────────────────────────┬───────────────────────────────────────┘   │
└─────────────────────────────────┼───────────────────────────────────────────┘
                                  ▼
                     ┌─────────────────────────┐
                     │  Base de datos analítica │  SistemaVentasETL
                     │  (Dimensiones + Hechos)  │  (DWH esquema estrella)
                     └─────────────────────────┘
                                  │
                     ┌─────────────▼─────────────┐
                     │  AnalisisVentas.Api       │  Módulo de visualización /
                     │  (Web API para dashboard) │  consultas del DWH
                     └───────────────────────────┘
```

### Fuentes de datos
| Fuente | Origen | Tecnología |
|---|---|---|
| CSV | `C:\MisBasesDeDatosVentasETL\Csv\Productos.csv`, `Clientes.csv` | CsvHelper |
| Base de datos relacional | Tablas `Products`, `Categories`, `Customers`, `Cities`, `Orders`, `Order_Details` | ADO.NET (`Microsoft.Data.SqlClient`) |
| API REST | `https://fakestoreapi.com/users` (suplidores) | `IHttpClientFactory` |

### Base de datos analítica (DWH)
- **Dimensiones**: `Dimensiones.DimProducto`, `DimCliente`, `DimSuplidor`, `DimFecha` (1,461 fechas: 2023–2026).
- **Hechos**: `Hechos.FactVentas`.
- Estrategia **SCD Tipo 1** (UPSERT: inserta si no existe, actualiza si existe).
- **Anti-duplicado** en la tabla de hechos (evita insertar la misma venta dos veces).

---

## Estructura del repositorio

```
AnalisisVentas.slnx
├── AnalisisVentas.Api/                 → Web API (módulo de visualización/consultas)
├── AnalisisVentas.Data/
│   ├── Class/FileFactory.cs            → Patrón Factory para lectura de archivos
│   ├── Entities/                        → POCOs de origen (Csv, Db, Api) y DWH
│   ├── Interfaces/                      → IExtractor<T>, IFileReaderRepository<T>,
│   │                                     IApiReaderRepository<T>, IDbReaderRepository<T>,
│   │                                     IDbWriterRepository<T>, IStagingService, ILoggerService
│   ├── Persistence/
│   │   ├── Repositories/                → Implementaciones por fuente (Csv, Api, Db, Dwh)
│   │   └── Staging/StagingService.cs    → Persistencia de la extracción en JSON
│   └── Services/
│       ├── EtlOrchestratorService.cs    → Orquesta el pipeline (E → staging → T+L)
│       ├── TransformService.cs          → Mapeos origen → DWH
│       ├── Extractors/                  → CsvExtractor, DatabaseExtractor, ApiExtractor
│       └── LoggerService.cs             → Logging y métricas (monitoreo)
├── AnalisisVentas.WkService/
│   ├── Program.cs                       → Composition Root (inyección de dependencias)
│   ├── Worker.cs                        → BackgroundService que ejecuta el pipeline
│   └── appsettings.json                 → Configuración centralizada
├── Database/
│   ├── SistemaVentasETL.sql             → BD transaccional + datos de muestra
│   └── SistemaVentasETL_DWH.sql         → DWH (esquemas, dimensiones, hechos, DimFecha)
└── Data/Csv/                            → Copia de los archivos CSV de origen
```

---

## Configuración y ejecución

### Requisitos
- .NET 8 SDK
- SQL Server Express (instancia `.\SQLEXPRESS`)
- Acceso a internet (para la API de suplidores)

### Pasos
1. **Crear la base de datos transaccional**: ejecutar `Database/SistemaVentasETL.sql` en SQL Server (crea la BD, las tablas y los datos de muestra).
2. **Crear el Data Warehouse**: ejecutar `Database/SistemaVentasETL_DWH.sql` (crea esquemas `Dimensiones`/`Hechos`, las tablas y puebla `DimFecha`).
3. **Preparar los CSV** (ruta configurable en `appsettings.json` → `CsvPaths`):
   - Copiar `Data/Csv/Productos.csv` y `Data/Csv/Clientes.csv` a `C:\MisBasesDeDatosVentasETL\Csv\` (o ajustar la ruta en la configuración).
4. **Configurar la conexión** en `appsettings.json` → `ConnectionStrings:SistemaVentasETL`. El valor por defecto usa autenticación de Windows (`Trusted_Connection`), sin contraseñas en el archivo.
5. **Ejecutar el worker**: `dotnet run --project AnalisisVentas.WkService`.

> Nota: los archivos de staging se escriben en `C:\MisBasesDeDatosVentasETL\Staging\` (configurable en `Staging:Directory`).

---

## Flujo del proceso ETL

1. **Extracción (E)** — `EtlOrchestratorService` ejecuta en **paralelo** (`Task.WhenAll`) los 9 extractores:
   - 6 tablas de la BD relacional (Productos, Categorías, Clientes, Ciudades, Órdenes, Detalles).
   - 1 API REST (Suplidores).
   - 2 archivos CSV (Productos, Clientes).
   - Las fuentes de BD son **críticas** (si fallan se aborta el proceso); API y CSV son **tolerantes a fallos** (continúan con datos vacíos).
2. **Staging** — los datos extraídos se persisten en archivos temporales JSON (`StagingService`).
3. **Transformación (T)** — `TransformService` mapea cada origen a las entidades del DWH.
4. **Carga (L)** — los escritores (`IDbWriterRepository<T>`) hacen UPSERT (SCD Tipo 1):
   - `DimProducto`, `DimCliente`, `DimSuplidor` se cargan en paralelo (`Parallel.ForEachAsync`, grado 4).
   - `FactVentas` se carga secuencialmente para preservar la **idempotencia del anti-duplicado**.
5. **Métricas** — el proceso registra tiempos con `Stopwatch` (extracción, staging, carga, total).

---

## Validación de atributos de calidad

| Atributo | Cómo lo garantiza |
|---|---|
| **Rendimiento** | Extracción en paralelo (`Task.WhenAll`), carga de dimensiones con `Parallel.ForEachAsync`, uso de `async/await` en toda la E/S, y medición de tiempos con `Stopwatch` (métricas `[METRICA: ...]` en el log). |
| **Escalabilidad** | Configuración modular de fuentes en `appsettings.json` + la interfaz `IExtractor<T>` permiten agregar nuevas fuentes (CSV, BD, API) sin modificar el orquestador. |
| **Seguridad** | Credenciales sin contraseñas en el código: la conexión usa autenticación de Windows (`Trusted_Connection`), y las rutas/URLs están centralizadas en `appsettings.json` (nunca hardcodeadas). |
| **Mantenibilidad** | Clean Architecture con separación por capas (Data, Api, Worker), principios SOLID, patrones Repository, Factory, interfaces de abstracción e inyección de dependencias por constructor (Composition Root en `Program.cs`). |

---

## Tecnologías
- **.NET 8** (Worker Service, Web API, class library)
- **CsvHelper** (lectura de CSV)
- **ADO.NET** (`Microsoft.Data.SqlClient`) para la extracción y carga
- **IHttpClientFactory** para el consumo de la API
- **SQL Server Express** (BD transaccional y DWH)
