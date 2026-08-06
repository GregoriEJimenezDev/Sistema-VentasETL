using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AnalisisVentas.Data.Domain.Interfaces;

namespace AnalisisVentas.Data.Infrastructure.Persistence.Repositories;

public class CsvVentasFileReaderRepository : ICsvVentasFileReaderRepository
{
    public async Task<IEnumerable<VentaCsvRow>> ReadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var records = new List<VentaCsvRow>();
        
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            MissingFieldFound = null,
            BadDataFound = null
        });

        await foreach (var record in csv.GetRecordsAsync<VentaCsvRow>(cancellationToken))
        {
            records.Add(record);
        }

        return records;
    }
}