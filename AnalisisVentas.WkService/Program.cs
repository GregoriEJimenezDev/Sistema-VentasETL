using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Entities.Csv;
using AnalisisVentas.Data.Entities.Db;
using AnalisisVentas.Data.Entities.Dwh.Dimensions;
using AnalisisVentas.Data.Entities.Dwh.Facts;
using AnalisisVentas.Data.Interfaces;
using AnalisisVentas.Data.Persistence.Repositories.Api;
using AnalisisVentas.Data.Persistence.Repositories.Csv;
using AnalisisVentas.Data.Persistence.Repositories.Db;
using AnalisisVentas.Data.Persistence.Repositories.Dwh;
using AnalisisVentas.Data.Persistence.Staging;
using AnalisisVentas.Data.Services;
using AnalisisVentas.Data.Services.Extractors;
using AnalisisVentas.WkService;

var builder = Host.CreateApplicationBuilder(args);

// Principio D: Composition Root, se registran las implementaciones contra sus interfaces.
// Se usa AddTransient porque el Worker (BackgroundService singleton) no puede consumir
// servicios scoped; AddScoped rompería el runtime. AddSingleton para LoggerService (stateless).

builder.Services.AddHttpClient();

// Servicios transversales
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddTransient<IStagingService, StagingService>();

// Lectores CSV (archivos planos)
builder.Services.AddTransient<IFileReaderRepository<ProductoCsv>, CsvVentasFileReaderRepository<ProductoCsv>>();
builder.Services.AddTransient<IFileReaderRepository<ClienteCsv>, CsvVentasFileReaderRepository<ClienteCsv>>();

// Lectores API
builder.Services.AddTransient<IApiReaderRepository<Supplier>, ApiSuplidorReaderRepository>();

// Lectores BD transaccional
builder.Services.AddTransient<IDbReaderRepository<Product>, DbProductReaderRepository>();
builder.Services.AddTransient<IDbReaderRepository<Category>, DbCategoryReaderRepository>();
builder.Services.AddTransient<IDbReaderRepository<Customer>, DbCustomerReaderRepository>();
builder.Services.AddTransient<IDbReaderRepository<City>, DbCityReaderRepository>();
builder.Services.AddTransient<IDbReaderRepository<Order>, DbOrderReaderRepository>();
builder.Services.AddTransient<IDbReaderRepository<OrderDetail>, DbVentasReaderRepository>();

// Extractores (interfaz IExtractor + implementación por tipo de fuente)
builder.Services.AddTransient<IExtractor<ProductoCsv>>(sp => new CsvExtractor<ProductoCsv>(
    sp.GetRequiredService<IFileReaderRepository<ProductoCsv>>(),
    sp.GetRequiredService<IConfiguration>()["CsvPaths:Productos"]!,
    sp.GetRequiredService<ILoggerService>()));

builder.Services.AddTransient<IExtractor<ClienteCsv>>(sp => new CsvExtractor<ClienteCsv>(
    sp.GetRequiredService<IFileReaderRepository<ClienteCsv>>(),
    sp.GetRequiredService<IConfiguration>()["CsvPaths:Clientes"]!,
    sp.GetRequiredService<ILoggerService>()));

builder.Services.AddTransient<IExtractor<Supplier>>(sp => new ApiExtractor<Supplier>(
    sp.GetRequiredService<IApiReaderRepository<Supplier>>(),
    sp.GetRequiredService<IConfiguration>()["ApiSettings:SuppliersUrl"]!,
    sp.GetRequiredService<ILoggerService>()));

builder.Services.AddTransient<IExtractor<Product>, DatabaseExtractor<Product>>();
builder.Services.AddTransient<IExtractor<Category>, DatabaseExtractor<Category>>();
builder.Services.AddTransient<IExtractor<Customer>, DatabaseExtractor<Customer>>();
builder.Services.AddTransient<IExtractor<City>, DatabaseExtractor<City>>();
builder.Services.AddTransient<IExtractor<Order>, DatabaseExtractor<Order>>();
builder.Services.AddTransient<IExtractor<OrderDetail>, DatabaseExtractor<OrderDetail>>();

// Escritores DWH
builder.Services.AddTransient<IDbWriterRepository<DimProducto>, DimProductoWriterRepository>();
builder.Services.AddTransient<IDbWriterRepository<DimCliente>, DimClienteWriterRepository>();
builder.Services.AddTransient<IDbWriterRepository<DimSuplidor>, DimSuplidorWriterRepository>();
builder.Services.AddTransient<IDbWriterRepository<FactVentas>, FactVentasWriterRepository>();

// Orquestador del pipeline ETL + Worker
builder.Services.AddTransient<EtlOrchestratorService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
