using System.Globalization;
using ParkingPredictor.Domain;

namespace ParkingPredictor.Collector;

// Ölçümleri iki tür CSV dosyasına yazar:
//   parks.csv                 → otoparkların değişmeyen bilgileri, her çalıştırmada baştan yazılır
//   snapshots/yyyy-MM-dd.csv  → o günün (UTC) ölçümleri, her çalıştırmada sonuna eklenir
public static class CsvStorage
{
    private const string ParksHeader =
        "id,name,latitude,longitude,capacity,type,district,work_hours,free_minutes";
    private const string SnapshotsHeader =
        "collected_at_utc,park_id,empty_capacity,is_open";

    private static readonly char[] CharactersNeedingQuotes = [',', '"', '\n', '\r'];

    public static void WriteParks(string dataDirectory, IEnumerable<ParkingLot> parkingLots)
    {
        Directory.CreateDirectory(dataDirectory);
        var path = Path.Combine(dataDirectory, "parks.csv");

        var lines = new List<string> { ParksHeader };
        // Id'ye göre sıralı yazıyoruz: bilgiler değişmediyse dosya da birebir aynı kalır,
        // böylece Git yalnızca gerçek değişiklikleri kaydeder.
        foreach (var lot in parkingLots.OrderBy(p => p.Id))
        {
            lines.Add(string.Join(",",
                lot.Id.ToString(CultureInfo.InvariantCulture),
                Escape(lot.Name),
                lot.Latitude.ToString(CultureInfo.InvariantCulture),
                lot.Longitude.ToString(CultureInfo.InvariantCulture),
                lot.Capacity.ToString(CultureInfo.InvariantCulture),
                lot.Type.ToString(),
                Escape(lot.District),
                Escape(lot.WorkHours),
                lot.FreeMinutes.ToString(CultureInfo.InvariantCulture)));
        }

        File.WriteAllText(path, JoinLines(lines));
    }

    public static void AppendSnapshots(
        string dataDirectory, DateTimeOffset collectedAtUtc, IEnumerable<OccupancySnapshot> snapshots)
    {
        var directory = Path.Combine(dataDirectory, "snapshots");
        Directory.CreateDirectory(directory);

        var fileName = collectedAtUtc.UtcDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".csv";
        var path = Path.Combine(directory, fileName);

        var lines = new List<string>();
        if (!File.Exists(path))
        {
            lines.Add(SnapshotsHeader);   // Başlık yalnızca günün ilk ölçümünde yazılır.
        }

        foreach (var snapshot in snapshots.OrderBy(s => s.ParkingLotId))
        {
            lines.Add(string.Join(",",
                snapshot.CollectedAtUtc.UtcDateTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture),
                snapshot.ParkingLotId.ToString(CultureInfo.InvariantCulture),
                snapshot.EmptyCapacity.ToString(CultureInfo.InvariantCulture),
                snapshot.IsOpen ? "true" : "false"));
        }

        File.AppendAllText(path, JoinLines(lines));
    }

    // Satır sonunu her işletim sisteminde "\n" olarak sabitler (Windows'ta varsayılan "\r\n").
    private static string JoinLines(List<string> lines) => string.Join("\n", lines) + "\n";

    // CSV kuralı: virgül, tırnak ya da satır sonu içeren değer tırnağa alınır, içteki tırnak ikilenir.
    private static string Escape(string value)
    {
        if (value.IndexOfAny(CharactersNeedingQuotes) < 0)
        {
            return value;
        }

        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }
}
