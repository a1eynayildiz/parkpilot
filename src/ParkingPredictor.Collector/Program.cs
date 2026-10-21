using ParkingPredictor.Collector;
using ParkingPredictor.Domain;

using var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
var client = new IsparkClient(httpClient);

var dtos = await client.GetParksAsync();
var collectedAtUtc = DateTimeOffset.UtcNow;

var parkingLots = new List<ParkingLot>();
var snapshots = new List<OccupancySnapshot>();

foreach (var dto in dtos)
{
    try
    {
        parkingLots.Add(IsparkMapper.ToParkingLot(dto));
        snapshots.Add(IsparkMapper.ToSnapshot(dto, collectedAtUtc));
    }
    catch (FormatException)
    {
        Console.Error.WriteLine($"Atlandı: {dto.ParkId} numaralı otoparkın koordinatı okunamadı ({dto.Lat}, {dto.Lng}).");
    }
}

Console.WriteLine($"{collectedAtUtc:u} itibarıyla {parkingLots.Count} otopark okundu.");
Console.WriteLine($"Türü tanınmayan otopark sayısı: {parkingLots.Count(p => p.Type == ParkingLotType.Unknown)}");
