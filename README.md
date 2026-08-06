# Sistema Insertar Ventas - Proceso ETL

**Autor:** Gregori Evangelista Jimenez  
**Institución:** Instituto Tecnológico de Las Américas (ITLA)  

## Descripción del Proyecto
Este proyecto implementa un proceso ETL (Extracción, Transformación y Carga) robusto desarrollado en C# .NET 8. El sistema está diseñado bajo los principios de Clean Architecture y SOLID. 

Su objetivo principal es extraer datos transaccionales desde un archivo plano (`.csv`), limpiar y deduplicar la información en memoria utilizando LINQ, y finalmente cargar las dimensiones y la tabla de hechos en un Data Warehouse (SQL Server) utilizando un Modelo de Estrella (Star Schema).

## Características Principales
* **Extracción:** Lectura optimizada de archivos CSV.
* **Transformación:** Deduplicación de dimensiones (Categorías, Productos, Clientes, Suplidores) y generación automática de la dimensión de Tiempo (DimFecha).
* **Carga (UPSERT):** Implementación de lógica idempotente con Entity Framework Core para actualizar registros existentes sin violar restricciones de llaves primarias.
* **Tolerancia a fallos:** Manejo global de excepciones para mantener el ciclo de vida del Worker Service estable.

## Tecnologías Utilizadas
* **Lenguaje/Framework:** C# .NET 8 (Worker Service)
* **ORM:** Entity Framework Core
* **Base de Datos:** Microsoft SQL Server
* **Arquitectura:** Clean Architecture (Capas: Domain, Application, Infrastructure)
