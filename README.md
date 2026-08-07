fo: ETLVentas.DW.workerLoad.Worker[0]
      === INICIO DEL ETL COMPLETO (extracción + carga) ===
info: ETLVentas.DW.workerLoad.Worker[0]
      Fase 1: Extracción de datos hacia staging...
info: AnalisisVentas.ETL[0]
      === INICIO DEL PROCESO ETL - 08/07/2026 18:37:33 ===
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
info: ETLVentas.DW.persistencia.Repositories.Csv.CsvVentasFileReaderRepository[0]
      Lectura del archivo CSV completada: C:\MisBasesDeDatosVentasETL\Csv\Clientes.csv - 10 registros
info: ETLVentas.DW.persistencia.Repositories.Csv.CsvVentasFileReaderRepository[0]
      Lectura del archivo CSV completada: C:\MisBasesDeDatosVentasETL\Csv\Productos.csv - 11 registros
info: ETLVentas.DW.persistencia.Repositories.Db.DbOrderReaderRepository[0]
      Lectura de Orders completada - 16 órdenes
info: ETLVentas.DW.persistencia.Repositories.Db.DbCityReaderRepository[0]
      Lectura de Cities completada - 5 ciudades
info: ETLVentas.DW.persistencia.Repositories.Db.DbVentasReaderRepository[0]
      Lectura de ventas desde la base de datos completada - 18 detalles
info: ETLVentas.DW.persistencia.Repositories.Db.DbProductReaderRepository[0]
      Lectura de Products completada - 11 productos
info: ETLVentas.DW.persistencia.Repositories.Db.DbCustomerReaderRepository[0]
      Lectura de Customers completada - 10 clientes
info: ETLVentas.DW.persistencia.Repositories.Db.DbCategoryReaderRepository[0]
      Lectura de Categories completada - 2 categorías
info: AnalisisVentas.ETL[0]
      CsvExtractor: extraídos 10 registros desde C:\MisBasesDeDatosVentasETL\Csv\Clientes.csv
info: AnalisisVentas.ETL[0]
      CsvExtractor: extraídos 11 registros desde C:\MisBasesDeDatosVentasETL\Csv\Productos.csv
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 2 registros de Category
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 11 registros de Product
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 16 registros de Order
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 5 registros de City
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 18 registros de OrderDetail
info: AnalisisVentas.ETL[0]
      DatabaseExtractor: extraídos 10 registros de Customer
fail: ETLVentas.DW.persistencia.Repositories.Api.ApiSuplidorReaderRepository[0]
      Error al consultar suplidores a la API: http://localhost:5082/api/suplidores
      System.Net.Http.HttpRequestException: No connection could be made because the target machine actively refused it. (localhost:5082)
       ---> System.Net.Sockets.SocketException (10061): No connection could be made because the target machine actively refused it.
         at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.ThrowException(SocketError error, CancellationToken cancellationToken)
         at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.System.Threading.Tasks.Sources.IValueTaskSource.GetResult(Int16 token)
         at System.Net.Sockets.Socket.<ConnectAsync>g__WaitForConnectWithCancellation|285_0(AwaitableSocketAsyncEventArgs saea, ValueTask connectTask, CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.ConnectToTcpHostAsync(String host, Int32 port, HttpRequestMessage initialRequest, Boolean async, CancellationToken cancellationToken)
         --- End of inner exception stack trace ---
         at System.Net.Http.HttpConnectionPool.ConnectToTcpHostAsync(String host, Int32 port, HttpRequestMessage initialRequest, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.ConnectAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.CreateHttp11ConnectionAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.AddHttp11ConnectionAsync(QueueItem queueItem)
         at System.Threading.Tasks.TaskCompletionSourceWithCancellation`1.WaitWithCancellationAsync(CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.HttpConnectionWaiter`1.WaitForConnectionWithTelemetryAsync(HttpRequestMessage request, HttpConnectionPool pool, Boolean async, CancellationToken requestCancellationToken)
         at System.Net.Http.HttpConnectionPool.SendWithVersionDetectionAndRetryAsync(HttpRequestMessage request, Boolean async, Boolean doRequestAuth, CancellationToken cancellationToken)
         at System.Net.Http.DiagnosticsHandler.SendAsyncCore(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.RedirectHandler.SendAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at Microsoft.Extensions.Http.Logging.LoggingHttpMessageHandler.<SendCoreAsync>g__Core|5_0(HttpRequestMessage request, Boolean useAsync, CancellationToken cancellationToken)
         at Microsoft.Extensions.Http.Logging.LoggingScopeHttpMessageHandler.<SendCoreAsync>g__Core|5_0(HttpRequestMessage request, Boolean useAsync, CancellationToken cancellationToken)
         at System.Net.Http.HttpClient.<SendAsync>g__Core|83_0(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationTokenSource cts, Boolean disposeCts, CancellationTokenSource pendingRequestsCts, CancellationToken originalCancellationToken)
         at ETLVentas.DW.persistencia.Repositories.Api.ApiSuplidorReaderRepository.ReadFromApiAsync(String url) in C:\Users\user\source\repos\GregoriEJimenezDev\Sistema-VentasETL\src\ETLVentas.DW.persistencia\Repositories\Api\ApiSuplidorReaderRepository.cs:line 33
fail: AnalisisVentas.ETL[0]
      Fallo al extraer Suplidores (API). Se omite.
      System.Net.Http.HttpRequestException: No connection could be made because the target machine actively refused it. (localhost:5082)
       ---> System.Net.Sockets.SocketException (10061): No connection could be made because the target machine actively refused it.
         at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.ThrowException(SocketError error, CancellationToken cancellationToken)
         at System.Net.Sockets.Socket.AwaitableSocketAsyncEventArgs.System.Threading.Tasks.Sources.IValueTaskSource.GetResult(Int16 token)
         at System.Net.Sockets.Socket.<ConnectAsync>g__WaitForConnectWithCancellation|285_0(AwaitableSocketAsyncEventArgs saea, ValueTask connectTask, CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.ConnectToTcpHostAsync(String host, Int32 port, HttpRequestMessage initialRequest, Boolean async, CancellationToken cancellationToken)
         --- End of inner exception stack trace ---
         at System.Net.Http.HttpConnectionPool.ConnectToTcpHostAsync(String host, Int32 port, HttpRequestMessage initialRequest, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.ConnectAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.CreateHttp11ConnectionAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.AddHttp11ConnectionAsync(QueueItem queueItem)
         at System.Threading.Tasks.TaskCompletionSourceWithCancellation`1.WaitWithCancellationAsync(CancellationToken cancellationToken)
         at System.Net.Http.HttpConnectionPool.HttpConnectionWaiter`1.WaitForConnectionWithTelemetryAsync(HttpRequestMessage request, HttpConnectionPool pool, Boolean async, CancellationToken requestCancellationToken)
         at System.Net.Http.HttpConnectionPool.SendWithVersionDetectionAndRetryAsync(HttpRequestMessage request, Boolean async, Boolean doRequestAuth, CancellationToken cancellationToken)
         at System.Net.Http.DiagnosticsHandler.SendAsyncCore(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at System.Net.Http.RedirectHandler.SendAsync(HttpRequestMessage request, Boolean async, CancellationToken cancellationToken)
         at Microsoft.Extensions.Http.Logging.LoggingHttpMessageHandler.<SendCoreAsync>g__Core|5_0(HttpRequestMessage request, Boolean useAsync, CancellationToken cancellationToken)
         at Microsoft.Extensions.Http.Logging.LoggingScopeHttpMessageHandler.<SendCoreAsync>g__Core|5_0(HttpRequestMessage request, Boolean useAsync, CancellationToken cancellationToken)
         at System.Net.Http.HttpClient.<SendAsync>g__Core|83_0(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationTokenSource cts, Boolean disposeCts, CancellationTokenSource pendingRequestsCts, CancellationToken originalCancellationToken)
         at ETLVentas.DW.persistencia.Repositories.Api.ApiSuplidorReaderRepository.ReadFromApiAsync(String url) in C:\Users\user\source\repos\GregoriEJimenezDev\Sistema-VentasETL\src\ETLVentas.DW.persistencia\Repositories\Api\ApiSuplidorReaderRepository.cs:line 33
         at ETLVentas.DW.application.Services.Extractors.ApiExtractor`1.ExtractAsync(CancellationToken cancellationToken) in C:\Users\user\source\repos\GregoriEJimenezDev\Sistema-VentasETL\src\ETLVentas.DW.application\Services\Extractors\ApiExtractor.cs:line 23
         at ETLVentas.DW.application.Services.EtlOrchestratorService.GuardAsync[T](Task`1 tarea, String fuente) in C:\Users\user\source\repos\GregoriEJimenezDev\Sistema-VentasETL\src\ETLVentas.DW.application\Services\EtlOrchestratorService.cs:line 116
info: AnalisisVentas.ETL[0]
      [METRICA: Extraccion] Productos: 11 | Categorías: 2 | Clientes: 10 | Ciudades: 5 | Ordenes: 16 | Detalles: 18 | Suplidores API: 0 | Productos CSV: 11 | Clientes CSV: 10 - Tiempo: 00:00:04.6401675
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
      Staging: 0 registros de suplidores-api escritos en C:\MisBasesDeDatosVentasETL\Staging\suplidores-api.json
info: AnalisisVentas.ETL[0]
      Staging: 11 registros de productos-csv escritos en C:\MisBasesDeDatosVentasETL\Staging\productos-csv.json
info: AnalisisVentas.ETL[0]
      Staging: 10 registros de clientes-csv escritos en C:\MisBasesDeDatosVentasETL\Staging\clientes-csv.json
info: AnalisisVentas.ETL[0]
      [METRICA: Staging] 9 conjuntos de datos persistidos - Tiempo: 00:00:00.0873679
info: AnalisisVentas.ETL[0]
      === FIN DEL PROCESO ETL - Extracción total: 00:00:04.7378573 ===
info: ETLVentas.DW.workerLoad.Worker[0]
      Fase 2: Carga de ventas al DWH...
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      === INICIANDO PROCESO ETL ===
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Archivo origen: C:\MisBasesDeDatosVentasETL\MayCsv\VentasCompleto.csv
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 1: Leyendo archivo CSV...
info: ETLVentas.DW.persistencia.Repositories.VentasCsvFileReaderRepository[0]
      Iniciando lectura de archivo CSV: C:\MisBasesDeDatosVentasETL\MayCsv\VentasCompleto.csv
info: ETLVentas.DW.persistencia.Repositories.VentasCsvFileReaderRepository[0]
      Lectura completada. Registros válidos: 6
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 1 completado. Registros leídos: 6
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 2: Transformando y deduplicando dimensiones...
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Categorías únicas: 3
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Productos únicos: 6
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Clientes únicos: 5
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Suplidores únicos: 3
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 3: Generando dimensión de tiempo...
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Fechas únicas: 4
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 4: Cargando dimensiones en DWH...
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Iniciando carga de datos en DWH
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (12ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT 1
warn: Microsoft.EntityFrameworkCore.Query[10103]
      The query uses the 'First'/'FirstOrDefault' operator without 'OrderBy' and filter operators. This may lead to unpredictable results.
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (15ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT TOP(1) [t].[Value]
      FROM (
          SELECT CASE WHEN OBJECT_ID(N'Dimensiones.DimCategoria', N'U') IS NULL THEN 0 ELSE 1 END AS Value
      ) AS [t]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Base de datos verificada/creada
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimCategoria: 3 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[CategoriaKey], [d].[FechaCreacionDW], [d].[NombreCategoria]
      FROM [Dimensiones].[DimCategoria] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimProducto: 6 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ProductoKey], [d].[Categoria], [d].[Codigo], [d].[FechaCreacionDW], [d].[NombreProducto], [d].[Precio], [d].[Stock]
      FROM [Dimensiones].[DimProducto] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimCliente: 5 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (8ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ClienteKey], [d].[Ciudad], [d].[ClienteIdOrigen], [d].[Email], [d].[FechaCreacionDW], [d].[NombreCompleto], [d].[Telefono]
      FROM [Dimensiones].[DimCliente] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimSuplidor: 3 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[SuplidorKey], [d].[Ciudad], [d].[Email], [d].[FechaCreacionDW], [d].[NombreSuplidor], [d].[SuplidorIdOrigen], [d].[Telefono]
      FROM [Dimensiones].[DimSuplidor] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimFecha: 4 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (7ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[FechaKey]
      FROM [Dimensiones].[DimFecha] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando FactVentas: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [f].[ProductoKey], [f].[ClienteKey], [f].[FechaKey]
      FROM [Hechos].[FactVentas] AS [f]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Guardando cambios en base de datos...
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (56ms) [Parameters=[@p1='?' (DbType = Int32), @p0='?' (DbType = DateTime2), @p3='?' (DbType = Int32), @p2='?' (DbType = DateTime2), @p5='?' (DbType = Int32), @p4='?' (DbType = DateTime2), @p7='?' (DbType = Int32), @p6='?' (DbType = DateTime2), @p9='?' (DbType = Int32), @p8='?' (DbType = DateTime2), @p11='?' (DbType = Int32), @p10='?' (DbType = DateTime2), @p13='?' (DbType = Int32), @p12='?' (DbType = DateTime2), @p15='?' (DbType = Int32), @p14='?' (DbType = DateTime2), @p17='?' (DbType = Int32), @p16='?' (DbType = DateTime2), @p19='?' (DbType = Int32), @p18='?' (DbType = DateTime2), @p21='?' (DbType = Int32), @p20='?' (DbType = DateTime2), @p23='?' (DbType = Int32), @p22='?' (DbType = DateTime2), @p25='?' (DbType = Int32), @p24='?' (DbType = DateTime2), @p27='?' (DbType = Int32), @p26='?' (DbType = DateTime2), @p29='?' (DbType = Int32), @p28='?' (DbType = DateTime2), @p31='?' (DbType = Int32), @p30='?' (DbType = DateTime2), @p33='?' (DbType = Int32), @p32='?' (DbType = DateTime2)], CommandType='Text', CommandTimeout='120']
      SET NOCOUNT ON;
      UPDATE [Dimensiones].[DimCategoria] SET [FechaCreacionDW] = @p0
      OUTPUT 1
      WHERE [CategoriaKey] = @p1;
      UPDATE [Dimensiones].[DimCategoria] SET [FechaCreacionDW] = @p2
      OUTPUT 1
      WHERE [CategoriaKey] = @p3;
      UPDATE [Dimensiones].[DimCategoria] SET [FechaCreacionDW] = @p4
      OUTPUT 1
      WHERE [CategoriaKey] = @p5;
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
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p16
      OUTPUT 1
      WHERE [ProductoKey] = @p17;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p18
      OUTPUT 1
      WHERE [ProductoKey] = @p19;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p20
      OUTPUT 1
      WHERE [ProductoKey] = @p21;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p22
      OUTPUT 1
      WHERE [ProductoKey] = @p23;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p24
      OUTPUT 1
      WHERE [ProductoKey] = @p25;
      UPDATE [Dimensiones].[DimProducto] SET [FechaCreacionDW] = @p26
      OUTPUT 1
      WHERE [ProductoKey] = @p27;
      UPDATE [Dimensiones].[DimSuplidor] SET [FechaCreacionDW] = @p28
      OUTPUT 1
      WHERE [SuplidorKey] = @p29;
      UPDATE [Dimensiones].[DimSuplidor] SET [FechaCreacionDW] = @p30
      OUTPUT 1
      WHERE [SuplidorKey] = @p31;
      UPDATE [Dimensiones].[DimSuplidor] SET [FechaCreacionDW] = @p32
      OUTPUT 1
      WHERE [SuplidorKey] = @p33;
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Carga de datos completada exitosamente
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 4 completado. Dimensiones guardadas.
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Paso 5: Resolviendo claves foráneas y construyendo hechos...
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (7ms) [Parameters=[@__codigos_0='?' (Size = 4000)], CommandType='Text', CommandTimeout='120']
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
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (7ms) [Parameters=[@__nombres_0='?' (Size = 4000)], CommandType='Text', CommandTimeout='120']
      SELECT [d].[CategoriaKey], [d].[FechaCreacionDW], [d].[NombreCategoria]
      FROM [Dimensiones].[DimCategoria] AS [d]
      WHERE [d].[NombreCategoria] IN (
          SELECT [n].[value]
          FROM OPENJSON(@__nombres_0) WITH ([value] nvarchar(100) '$') AS [n]
      )
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (6ms) [Parameters=[@__idsOrigen_0='?' (Size = 4000)], CommandType='Text', CommandTimeout='120']
      SELECT [d].[SuplidorKey], [d].[Ciudad], [d].[Email], [d].[FechaCreacionDW], [d].[NombreSuplidor], [d].[SuplidorIdOrigen], [d].[Telefono]
      FROM [Dimensiones].[DimSuplidor] AS [d]
      WHERE [d].[SuplidorIdOrigen] IN (
          SELECT [i].[value]
          FROM OPENJSON(@__idsOrigen_0) WITH ([value] nvarchar(50) '$') AS [i]
      )
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
        - Hechos construidos: 6
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
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[CategoriaKey], [d].[FechaCreacionDW], [d].[NombreCategoria]
      FROM [Dimensiones].[DimCategoria] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimProducto: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ProductoKey], [d].[Categoria], [d].[Codigo], [d].[FechaCreacionDW], [d].[NombreProducto], [d].[Precio], [d].[Stock]
      FROM [Dimensiones].[DimProducto] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimCliente: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[ClienteKey], [d].[Ciudad], [d].[ClienteIdOrigen], [d].[Email], [d].[FechaCreacionDW], [d].[NombreCompleto], [d].[Telefono]
      FROM [Dimensiones].[DimCliente] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimSuplidor: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[SuplidorKey], [d].[Ciudad], [d].[Email], [d].[FechaCreacionDW], [d].[NombreSuplidor], [d].[SuplidorIdOrigen], [d].[Telefono]
      FROM [Dimensiones].[DimSuplidor] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando DimFecha: 0 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [d].[FechaKey]
      FROM [Dimensiones].[DimFecha] AS [d]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Procesando FactVentas: 6 registros
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='120']
      SELECT [f].[ProductoKey], [f].[ClienteKey], [f].[FechaKey]
      FROM [Hechos].[FactVentas] AS [f]
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Guardando cambios en base de datos...
info: ETLVentas.DW.persistencia.Repositories.SalesDwhRepository[0]
      Carga de datos completada exitosamente
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      === PROCESO ETL COMPLETADO EXITOSAMENTE ===
info: ETLVentas.DW.application.Services.VentasHandlerService[0]
      Resumen: 3 categorías, 6 productos, 5 clientes, 3 suplidores, 4 fechas, 6 hechos
info: ETLVentas.DW.workerLoad.Worker[0]
      === ETL COMPLETO FINALIZADO CORRECTAMENTE ===
info: ETLVentas.DW.workerLoad.Worker[0]
      ETL completado. Manteniendo el Worker vivo para revisión de logs. Presiona Ctrl+C para detener.
