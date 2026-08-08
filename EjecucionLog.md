ws [Version 10.0.26200.8875]
(c) Microsoft Corporation. All rights reserved.

C:\Users\user>cd C:\Users\user\source\repos\GregoriEJimenezDev\Sistema-VentasETL\src\ETLVentas.DW.workerLoad

C:\Users\user\source\repos\GregoriEJimenezDev\Sistema-VentasETL\src\ETLVentas.DW.workerLoad>dotnet run
Using launch settings from C:\Users\user\source\repos\GregoriEJimenezDev\Sistema-VentasETL\src\ETLVentas.DW.workerLoad\Properties\launchSettings.json...
Building...
info: ETLVentas.DW.workerLoad.Worker[0]
      === INICIO DEL ETL COMPLETO (extracción + carga) ===
info: ETLVentas.DW.workerLoad.Worker[0]
      Fase 1: Extracción de datos hacia staging...
info: AnalisisVentas.ETL[0]
      === INICIO DEL PROCESO ETL — 08/07/2026 20:14:28 ===
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: iniciando extracción de Product
info: ETLVentas.DW.persistencia.Repositories.Db.DbProductReaderRepository[0]
      Iniciando lectura de Products desde la base de datos
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: iniciando extracción de Category
info: ETLVentas.DW.persistencia.Repositories.Db.DbCategoryReaderRepository[0]
      Iniciando lectura de Categories desde la base de datos
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: iniciando extracción de Customer
info: ETLVentas.DW.persistencia.Repositories.Db.DbCustomerReaderRepository[0]
      Iniciando lectura de Customers desde la base de datos
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: iniciando extracción de City
info: ETLVentas.DW.persistencia.Repositories.Db.DbCityReaderRepository[0]
      Iniciando lectura de Cities desde la base de datos
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: iniciando extracción de Order
info: ETLVentas.DW.persistencia.Repositories.Db.DbOrderReaderRepository[0]
      Iniciando lectura de Orders desde la base de datos
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: iniciando extracción de OrderDetail
info: ETLVentas.DW.persistencia.Repositories.Db.DbVentasReaderRepository[0]
      Iniciando lectura de ventas desde la base de datos
info: AnalisisVentas.ETL[0]
      ApiExtractor: iniciando extracción desde http://localhost:5082/api/suplidores
info: ETLVentas.DW.persistencia.Repositories.Api.ApiSuplidorReaderRepository[0]
      Iniciando consulta de suplidores a la API: http://localhost:5082/api/suplidores
info: System.Net.Http.HttpClient.Default.LogicalHandler[100]
      Start processing HTTP request GET http://localhost:5082/api/suplidores
info: System.Net.Http.HttpClient.Default.ClientHandler[100]
      Sending HTTP request GET http://localhost:5082/api/suplidores
info: AnalisisVentas.ETL[0]
      CsvExtractor: iniciando extracción desde C:\MisBasesDeDatosVentasETL\Csv\Productos.csv
info: ETLVentas.DW.persistencia.Repositories.Csv.CsvVentasFileReaderRepository[0]
      Iniciando lectura del archivo CSV: C:\MisBasesDeDatosVentasETL\Csv\Productos.csv
info: AnalisisVentas.ETL[0]
      CsvExtractor: iniciando extracción desde C:\MisBasesDeDatosVentasETL\Csv\Clientes.csv
info: ETLVentas.DW.persistencia.Repositories.Csv.CsvVentasFileReaderRepository[0]
      Iniciando lectura del archivo CSV: C:\MisBasesDeDatosVentasETL\Csv\Clientes.csv
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\user\source\repos\GregoriEJimenezDev\Sistema-VentasETL\src\ETLVentas.DW.workerLoad
info: System.Net.Http.HttpClient.Default.ClientHandler[101]
      Received HTTP response headers after 49.6292ms - 200
info: System.Net.Http.HttpClient.Default.LogicalHandler[101]
      End processing HTTP request after 62.6887ms - 200
info: ETLVentas.DW.persistencia.Repositories.Api.ApiSuplidorReaderRepository[0]
      Consulta a la API completada: http://localhost:5082/api/suplidores — 5 suplidores
info: AnalisisVentas.ETL[0]
      ApiExtractor: extraídos 5 registros desde http://localhost:5082/api/suplidores
info: ETLVentas.DW.persistencia.Repositories.Csv.CsvVentasFileReaderRepository[0]
      Lectura del archivo CSV completada: C:\MisBasesDeDatosVentasETL\Csv\Clientes.csv — 10 registros
info: AnalisisVentas.ETL[0]
      CsvExtractor: extraídos 10 registros desde C:\MisBasesDeDatosVentasETL\Csv\Clientes.csv
info: ETLVentas.DW.persistencia.Repositories.Csv.CsvVentasFileReaderRepository[0]
      Lectura del archivo CSV completada: C:\MisBasesDeDatosVentasETL\Csv\Productos.csv — 11 registros
info: AnalisisVentas.ETL[0]
      CsvExtractor: extraídos 11 registros desde C:\MisBasesDeDatosVentasETL\Csv\Productos.csv
info: ETLVentas.DW.persistencia.Repositories.Db.DbCategoryReaderRepository[0]
      Lectura de Categories completada — 2 categorías
info: ETLVentas.DW.persistencia.Repositories.Db.DbCityReaderRepository[0]
      Lectura de Cities completada — 5 ciudades
info: ETLVentas.DW.persistencia.Repositories.Db.DbOrderReaderRepository[0]
      Lectura de Orders completada — 16 órdenes
info: ETLVentas.DW.persistencia.Repositories.Db.DbProductReaderRepository[0]
      Lectura de Products completada — 11 productos
info: ETLVentas.DW.persistencia.Repositories.Db.DbCustomerReaderRepository[0]
      Lectura de Customers completada — 10 clientes
info: ETLVentas.DW.persistencia.Repositories.Db.DbVentasReaderRepository[0]
      Lectura de ventas desde la base de datos completada — 18 detalles
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 11 registros de Product
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 10 registros de Customer
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 16 registros de Order
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 2 registros de Category
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 5 registros de City
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 18 registros de OrderDetail
info: AnalisisVentas.ETL[0]
      [METRICA: Extraccion] Productos: 11 | Categorías: 2 | Clientes: 10 | Ciudades: 5 | Órdenes: 16 | Detalles: 18 | Suplidores API: 5 | Productos CSV: 11 | Clientes CSV: 10 — Tiempo: 00:00:00.2561715
info: AnalisisVentas.ETL[0]
      Staging: 11 registros de productos-bd escritos en C:\MisBasesDeDatosVentasETL\Staging\productos-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 2 registros de categorias-bd escritos en C:\MisBasesDeDatosVentasETL\Staging\categorias-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 10 registros de clientes-bd escritos en C:\MisBasesDeDatosVentasETL\Staging\clientes-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 5 registros de ciudades-bd escritos en C:\MisBasesDeDatosVentasETL\Staging\ciudades-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 16 registros de ordenes-bd escritos en C:\MisBasesDeDatosVentasETL\Staging\ordenes-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 18 registros de detalles-bd escritos en C:\MisBasesDeDatosVentasETL\Staging\detalles-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 5 registros de suplidores-api escritos en C:\MisBasesDeDatosVentasETL\Staging\suplidores-api.json
info: AnalisisVentas.ETL[0]
      Staging: 11 registros de productos-csv escritos en C:\MisBasesDeDatosVentasETL\Staging\productos-csv.json
info: AnalisisVentas.ETL[0]
      Staging: 10 registros de clientes-csv escritos en C:\MisBasesDeDatosVentasETL\Staging\clientes-csv.json
info: AnalisisVentas.ETL[0]
      [METRICA: Staging] 9 conjuntos de datos persistidos — Tiempo: 00:00:00.0239843
info: AnalisisVentas.ETL[0]
      === FIN DEL PROCESO ETL — Extracción total: 00:00:00.2895834 ===
info: ETLVentas.DW.workerLoad.Worker[0]
      Fase 2: Carga de ventas al DWH desde staging...
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      === INICIANDO FASE 2: CARGA AL DWH DESDE STAGING ===
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 1: Leyendo staging de la Fase 1...
info: AnalisisVentas.ETL[0]
      Staging: 11 registros de productos-bd leídos desde C:\MisBasesDeDatosVentasETL\Staging\productos-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 2 registros de categorias-bd leídos desde C:\MisBasesDeDatosVentasETL\Staging\categorias-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 10 registros de clientes-bd leídos desde C:\MisBasesDeDatosVentasETL\Staging\clientes-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 5 registros de ciudades-bd leídos desde C:\MisBasesDeDatosVentasETL\Staging\ciudades-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 16 registros de ordenes-bd leídos desde C:\MisBasesDeDatosVentasETL\Staging\ordenes-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 18 registros de detalles-bd leídos desde C:\MisBasesDeDatosVentasETL\Staging\detalles-bd.json
info: AnalisisVentas.ETL[0]
      Staging: 5 registros de suplidores-api leídos desde C:\MisBasesDeDatosVentasETL\Staging\suplidores-api.json
info: AnalisisVentas.ETL[0]
      Staging: 11 registros de productos-csv leídos desde C:\MisBasesDeDatosVentasETL\Staging\productos-csv.json
info: AnalisisVentas.ETL[0]
      Staging: 10 registros de clientes-csv leídos desde C:\MisBasesDeDatosVentasETL\Staging\clientes-csv.json
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Staging leído -> Productos BD: 11 | Categorías: 2 | Clientes BD: 10 | Ciudades: 5 | Órdenes: 16 | Detalles: 18 | Suplidores API: 5 | Productos CSV: 11 | Clientes CSV: 10
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 2: Transformando y deduplicando dimensiones...
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Categorías únicas: 2
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Productos únicos: 11
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Clientes únicos: 10
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Suplidores únicos: 5
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 3: Generando dimensión de tiempo...
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Fechas únicas: 8
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 4: Cargando dimensiones en DWH...
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Iniciando carga de datos en DWH
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (6ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT 1
warn: Microsoft.EntityFrameworkCore.Query[10103]
      The query uses the 'First'/'FirstOrDefault' operator without 'OrderBy' and filter operators. This may lead to unpredictable results.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (14ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT TOP(1) [t].[Value]
      FROM (
          SELECT CASE WHEN OBJECT_ID(N'Dimensiones.DimCategoria', N'U') IS NULL THEN 0 ELSE 1 END AS Value
      ) AS [t]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Base de datos verificada/creada
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimCategoria: 2 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (6ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[CategoriaKey], [d].[FechaCreacionDW], [d].[NombreCategoria]
      FROM [Dimensiones].[DimCategoria] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimProducto: 11 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ProductoKey], [d].[Categoria], [d].[Codigo], [d].[FechaCreacionDW], [d].[NombreProducto], [d].[Precio], [d].[Stock]
      FROM [Dimensiones].[DimProducto] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimCliente: 10 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ClienteKey], [d].[Ciudad], [d].[ClienteIdOrigen], [d].[Email], [d].[FechaCreacionDW], [d].[NombreCompleto], [d].[Telefono]
      FROM [Dimensiones].[DimCliente] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimSuplidor: 5 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (3ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[SuplidorKey], [d].[Ciudad], [d].[Email], [d].[FechaCreacionDW], [d].[NombreSuplidor], [d].[SuplidorIdOrigen], [d].[Telefono]
      FROM [Dimensiones].[DimSuplidor] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimFecha: 8 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[FechaKey]
      FROM [Dimensiones].[DimFecha] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando FactVentas: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [f].[ProductoKey], [f].[ClienteKey], [f].[FechaKey]
      FROM [Hechos].[FactVentas] AS [f]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Guardando cambios en base de datos...
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (66ms) [Parameters=[@p1='?' (DbType = Int32), @p0='?' (DbType = DateTime2), @p3='?' (DbType = Int32), @p2='?' (DbType = DateTime2), @p5='?' (DbType = Int32), @p4='?' (DbType = DateTime2), @p7='?' (DbType = Int32), @p6='?' (DbType = DateTime2), @p9='?' (DbType = Int32), @p8='?' (DbType = DateTime2), @p11='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p13='?' (DbType = Int32), @p12='?' (DbType = DateTime2), @p15='?' (DbType = Int32), @p14='?' (DbType = DateTime2), @p17='?' (DbType = Int32), @p16='?' (DbType = DateTime2), @p19='?' (DbType = Int32), @p18='?' (DbType = DateTime2), @p21='?' (DbType = Int32), @p20='?' (DbType = DateTime2), @p23='?' (DbType = Int32), @p22='?' (DbType = DateTime2), @p25='?' (DbType = Int32), @p24='?' (DbType = DateTime2), @p27='?' (DbType = Int32), @p26='?' (DbType = DateTime2), @p29='?' (DbType = Int32), @p28='?' (DbType = DateTime2), @p31='?' (DbType = Int32), @p30='?' (DbType = DateTime2), @p33='?' (DbType = Int32), @p32='?' (DbType = DateTime2), @p35='?' (DbType = Int32), @p34='?' (DbType = DateTime2), @p37='?' (DbType = Int32), @p36='?' (DbType = DateTime2), @p39='?' (DbType = Int32), @p38='?' (DbType = DateTime2), @p41='?' (DbType = Int32), @p40='?' (DbType = DateTime2), @p43='?' (DbType = Int32), @p42='?' (DbType = DateTime2), @p45='?' (DbType = Int32), @p44='?' (DbType = DateTime2), @p47='?' (DbType = Int32), @p46='?' (DbType = DateTime2), @p49='?' (DbType = Int32), @p48='?' (DbType = DateTime2), @p51='?' (DbType = Int32), @p50='?' (DbType = DateTime2), @p53='?' (DbType = Int32), @p52='?' (DbType = DateTime2), @p55='?' (DbType = Int32), @p54='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='120']
      SET NOCOUNT ON;
      UPDATE [Dimensiones].[DimCategoria] SET [FechaCreacionDW] = @p0
      OUTPUT 1
      WHERE [CategoriaKey] = @p1;
      UPDATE [Dimensiones].[DimCategoria] SET [FechaCreacionDW] = @p2
      OUTPUT 1
      WHERE [CategoriaKey] = @p3;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p4
      OUTPUT 1
      WHERE [ClienteKey] = @p5;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p6
      OUTPUT 1
      WHERE [ClienteKey] = @p7;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p8
      OUTPUT 1
      WHERE [ClienteKey] = @p9;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p10
      OUTPUT 1
      WHERE [ClienteKey] = @p11;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p12
      OUTPUT 1
      WHERE [ClienteKey] = @p13;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p14
      OUTPUT 1
      WHERE [ClienteKey] = @p15;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p16
      OUTPUT 1
      WHERE [ClienteKey] = @p17;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p18
      OUTPUT 1
      WHERE [ClienteKey] = @p19;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p20
      OUTPUT 1
      WHERE [ClienteKey] = @p21;
      UPDATE [Dimensiones].[DimCliente] SET [FechaCreacionDW] = @p22
      OUTPUT 1
      WHERE [ClienteKey] = @p23;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p24
      OUTPUT 1
      WHERE [ProductoKey] = @p25;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p26
      OUTPUT 1
      WHERE [ProductoKey] = @p27;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p28
      OUTPUT 1
      WHERE [ProductoKey] = @p29;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p30
      OUTPUT 1
      WHERE [ProductoKey] = @p31;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p32
      OUTPUT 1
      WHERE [ProductoKey] = @p33;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p34
      OUTPUT 1
      WHERE [ProductoKey] = @p35;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p36
      OUTPUT 1
      WHERE [ProductoKey] = @p37;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p38
      OUTPUT 1
      WHERE [ProductoKey] = @p39;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p40
      OUTPUT 1
      WHERE [ProductoKey] = @p41;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p42
      OUTPUT 1
      WHERE [ProductoKey] = @p43;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p44
      OUTPUT 1
      WHERE [ProductoKey] = @p45;
      UPDATE [Dimensiones].[DimSuplidor] SET [FechaCreacionDW] = @p46
      OUTPUT 1
      WHERE [SuplidorKey] = @p47;
      UPDATE [Dimensiones].[DimSuplidor] SET [FechaCreacionDW] = @p48
      OUTPUT 1
      WHERE [SuplidorKey] = @p49;
      UPDATE [Dimensiones].[DimSuplidor] SET [FechaCreacionDW] = @p50
      OUTPUT 1
      WHERE [SuplidorKey] = @p51;
      UPDATE [Dimensiones].[DimSuplidor] SET [FechaCreacionDW] = @p52
      OUTPUT 1
      WHERE [SuplidorKey] = @p53;
      UPDATE [Dimensiones].[DimSuplidor] SET [FechaCreacionDW] = @p54
      OUTPUT 1
      WHERE [SuplidorKey] = @p55;
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Carga de datos completada exitosamente
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 4 completado. Dimensiones guardadas.
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 5: Resolviendo claves foráneas y construyendo hechos...
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (9ms) [Parameters=[@__codigos_0='?' (Size = 4000)], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ProductoKey], [d].[Categoria], [d].[Codigo], [d].[FechaCreacionDW], [d].[NombreProducto], [d].[Precio], [d].[Stock]
      FROM [Dimensiones].[DimProducto] AS [d]
      WHERE [d].[Codigo] IN (
          SELECT [c].[value]
          FROM OPENJSON(@__codigos_0) WITH ([value] nvarchar(50) '$') AS [c]
      )
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (6ms) [Parameters=[@__idsOrigen_0='?' (Size = 4000)], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ClienteKey], [d].[Ciudad], [d].[ClienteIdOrigen], [d].[Email], [d].[FechaCreacionDW], [d].[NombreCompleto], [d].[Telefono]
      FROM [Dimensiones].[DimCliente] AS [d]
      WHERE [d].[ClienteIdOrigen] IN (
          SELECT [i].[value]
          FROM OPENJSON(@__idsOrigen_0) WITH ([value] nvarchar(50) '$') AS [i]
      )
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Hechos construidos: 18
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 6: Guardando hechos en DWH...
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Iniciando carga de datos en DWH
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT 1
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT TOP(1) [t].[Value]
      FROM (
          SELECT CASE WHEN OBJECT_ID(N'Dimensiones.DimCategoria', N'U') IS NULL THEN 0 ELSE 1 END AS Value
      ) AS [t]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Base de datos verificada/creada
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimCategoria: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[CategoriaKey], [d].[FechaCreacionDW], [d].[NombreCategoria]
      FROM [Dimensiones].[DimCategoria] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimProducto: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ProductoKey], [d].[Categoria], [d].[Codigo], [d].[FechaCreacionDW], [d].[NombreProducto], [d].[Precio], [d].[Stock]
      FROM [Dimensiones].[DimProducto] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimCliente: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ClienteKey], [d].[Ciudad], [d].[ClienteIdOrigen], [d].[Email], [d].[FechaCreacionDW], [d].[NombreCompleto], [d].[Telefono]
      FROM [Dimensiones].[DimCliente] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimSuplidor: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[SuplidorKey], [d].[Ciudad], [d].[Email], [d].[FechaCreacionDW], [d].[NombreSuplidor], [d].[SuplidorIdOrigen], [d].[Telefono]
      FROM [Dimensiones].[DimSuplidor] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimFecha: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[FechaKey]
      FROM [Dimensiones].[DimFecha] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando FactVentas: 18 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [f].[ProductoKey], [f].[ClienteKey], [f].[FechaKey]
      FROM [Hechos].[FactVentas] AS [f]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Guardando cambios en base de datos...
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Carga de datos completada exitosamente
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      === FASE 2 COMPLETADA EXITOSAMENTE ===
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Resumen: 2 categorías, 11 productos, 10 clientes, 5 suplidores, 8 fechas, 18 hechos
info: ETLVentas.DW.workerLoad.Worker[0]
      === ETL COMPLETO FINALIZADO CORRECTAMENTE ===
info: ETLVentas.DW.workerLoad.Worker[0]
      ETL completado. Manteniendo el Worker vivo para revisión de logs. Presiona Ctrl+C para detener.



