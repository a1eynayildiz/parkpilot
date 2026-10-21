namespace ParkingPredictor.Domain;

public class ParkingLot
{
    public int Id { get; set; }                           // İSPARK'ın parkID değeri
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }                  // Enlem
    public double Longitude { get; set; }                 // Boylam
    public int Capacity { get; set; }
    public ParkingLotType Type { get; set; }
    public string District { get; set; } = string.Empty;
    public string WorkHours { get; set; } = string.Empty; // Ham metin, örn. "07:00-23:00"
    public int FreeMinutes { get; set; }                  // Ücretsiz park süresi (dakika)
}
