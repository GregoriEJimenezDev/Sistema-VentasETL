using ETLVentas.DW.application.Services;
using ETLVentas.DW.application.Services.Extractors;
using ETLVentas.DW.domain.Entities.Api;
using ETLVentas.DW.domain.Entities.Csv;
using ETLVentas.DW.domain.Entities.Db;
using ETLVentas.DW.domain.Interfaces;
using ETLVentas.DW.persistencia;
using ETLVentas.DW.persistencia.Repositories;
using ETLVentas.DW.persistencia.Repositories.Api;
using ETLVentas.DW.persistencia.Repositories.Csv;
using ETLVentas.DW.persistencia.Repositories.Db;
using ETLVentas.DW.persistencia.Staging;
using ETLVentas.DW.workerLoad;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient();

// Servicios transversales
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddTransient<IStagingService, StagingService>();

// Lectores CSV (archivos planos) - extracción
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

// Orquestador de extracción
builder.Services.AddTransient<EtlOrchestratorService>();

// Carga al DWH
// Nota de diseño: la Carga usa EF Core porque simplifica el manejo de relaciones y claves
// foráneas del modelo dimensional (UPSERT por clave natural, anti-duplicados de hechos),
// mientras que la Extracción usa ADO.NET para tener control directo sobre lecturas simples
// de fuentes heterogéneas (BD transaccional, API y CSV).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextPool<VentasDwhContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
        sqlOptions.CommandTimeout(120);
    });
}, poolSize: 32);

builder.Services.AddTransient<ISalesDwhRepository, SalesDwhRepository>();
builder.Services.AddScoped<VentasHandlerService>();

// Worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
