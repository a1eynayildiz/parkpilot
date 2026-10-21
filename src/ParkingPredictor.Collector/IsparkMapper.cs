using System.Globalization;
using ParkingPredictor.Domain;

namespace ParkingPredictor.Collector;

public static class IsparkMapper
{
    public static ParkingLot ToParkingLot(IsparkParkDto dto) => new()
    {
        Id = dto.ParkId,
        Name = dto.ParkName,
        Latitude = ParseCoordinate(dto.Lat),
        Longitude = ParseCoordinate(dto.Lng),
        Capacity = dto.Capacity,
        Type = ParseType(dto.ParkType),
        District = dto.District,
        WorkHours = dto.WorkHours,
        FreeMinutes = dto.FreeTime
    };

    public static OccupancySnapshot ToSnapshot(IsparkParkDto dto, DateTimeOffset collectedAtUtc) => new()
    {
        ParkingLotId = dto.ParkId,
        CollectedAtUtc = collectedAtUtc,
        EmptyCapacity = dto.EmptyCapacity,
        IsOpen = dto.IsOpen == 1
    };

    // Türkçe dil ayarında nokta binlik ayırıcıdır: "41.0246" sessizce 410246 olur.
    // InvariantCulture, değeri dil ayarından bağımsız okur.
    private static double ParseCoordinate(string value) =>
        double.Parse(value, CultureInfo.InvariantCulture);

    private static ParkingLotType ParseType(string value) => value switch
    {
        "AÇIK OTOPARK"   => ParkingLotType.Open,
        "KAPALI OTOPARK" => ParkingLotType.Indoor,
        "YOL ÜSTÜ"       => ParkingLotType.OnStreet,
        _                => ParkingLotType.Unknown
    };
}
