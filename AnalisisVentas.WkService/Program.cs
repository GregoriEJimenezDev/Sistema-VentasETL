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
using AnalisisVentas.WkService;

var builder = Host.CreateApplicationBuilder(args);

// Principio D: Composition Root, se registran las implementaciones contra sus interfaces.
// Se usa AddTransient porque el Worker (BackgroundService singleton) no puede consumir
// servicios scoped; AddScoped rompería el runtime.
builder.Services.AddHttpClient();

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

// Escritores DWH
builder.Services.AddTransient<IDbWriterRepository<DimProducto>, DimProductoWriterRepository>();
builder.Services.AddTransient<IDbWriterRepository<DimCliente>, DimClienteWriterRepository>();
builder.Services.AddTransient<IDbWriterRepository<DimSuplidor>, DimSuplidorWriterRepository>();
builder.Services.AddTransient<IDbWriterRepository<FactVentas>, FactVentasWriterRepository>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
