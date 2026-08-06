using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AnalisisVentas.Data.Domain.Interfaces;
using AnalisisVentas.Data.Infrastructure.Persistence;
using AnalisisVentas.Data.Infrastructure.Persistence.Repositories;
using AnalisisVentas.Data.Application.Services;
using AnalisisVentas.WkService;

var builder = Host.CreateApplicationBuilder(args);

// 1. Configuración de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("SistemaVentasETL")
    ?? throw new InvalidOperationException("Connection string 'SistemaVentasETL' not found.");

// 2. EF Core con DbContextPool (obligatorio)
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

// 3. Inyección de dependencias
builder.Services.AddTransient<ICsvVentasFileReaderRepository, CsvVentasFileReaderRepository>();
builder.Services.AddTransient<ISalesDwhRepository, SalesDwhRepository>();
builder.Services.AddTransient<VentasHandlerService>();

// 4. Worker Service
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();