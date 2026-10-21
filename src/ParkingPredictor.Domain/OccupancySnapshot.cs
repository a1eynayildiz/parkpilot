namespace ParkingPredictor.Domain;

public class OccupancySnapshot
{
    public int Id { get; set; }
    public int ParkingLotId { get; set; }                // Hangi otoparka ait
    public DateTimeOffset CollectedAtUtc { get; set; }   // Ölçümün yapıldığı an (UTC)
    public int EmptyCapacity { get; set; }               // Ham değer, düzeltilmeden
    public bool IsOpen { get; set; }
}
