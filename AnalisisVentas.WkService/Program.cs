using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Entities.Csv;
using AnalisisVentas.Data.Entities.Db;
using AnalisisVentas.Data.Interfaces;
using AnalisisVentas.Data.Persistence.Repositories.Api;
using AnalisisVentas.Data.Persistence.Repositories.Csv;
using AnalisisVentas.Data.Persistence.Repositories.Db;
using AnalisisVentas.WkService;

var builder = Host.CreateApplicationBuilder(args);

// Principio D: Composition Root, se registran las implementaciones contra sus interfaces.
builder.Services.AddHttpClient();
builder.Services.AddTransient<IFileReaderRepository<ProductoCsv>, CsvVentasFileReaderRepository<ProductoCsv>>();
builder.Services.AddTransient<IFileReaderRepository<ClienteCsv>, CsvVentasFileReaderRepository<ClienteCsv>>();
builder.Services.AddTransient<IApiReaderRepository<Supplier>, ApiSuplidorReaderRepository>();
builder.Services.AddTransient<IDbReaderRepository<OrderDetail>, DbVentasReaderRepository>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
