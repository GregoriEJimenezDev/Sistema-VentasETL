using AnalisisVentas.Data.Entities.Api;
using AnalisisVentas.Data.Interfaces;
using AnalisisVentas.Data.Persistence.Repositories.Api;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IApiReaderRepository<Supplier>, ApiSuplidorReaderRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
