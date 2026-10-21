using System.Text.Json.Serialization;

namespace ParkingPredictor.Collector;

// İSPARK API'sinden gelen JSON'u olduğu gibi karşılar.
// Tuhaflıkları (metin koordinat, 1/0 isOpen) burada kalır; temiz modele çeviri ayrı yapılır.
public class IsparkParkDto
{
    [JsonPropertyName("parkID")]        public int ParkId { get; set; }
    [JsonPropertyName("parkName")]      public string ParkName { get; set; } = string.Empty;
    [JsonPropertyName("lat")]           public string Lat { get; set; } = string.Empty;
    [JsonPropertyName("lng")]           public string Lng { get; set; } = string.Empty;
    [JsonPropertyName("capacity")]      public int Capacity { get; set; }
    [JsonPropertyName("emptyCapacity")] public int EmptyCapacity { get; set; }
    [JsonPropertyName("workHours")]     public string WorkHours { get; set; } = string.Empty;
    [JsonPropertyName("parkType")]      public string ParkType { get; set; } = string.Empty;
    [JsonPropertyName("freeTime")]      public int FreeTime { get; set; }
    [JsonPropertyName("district")]      public string District { get; set; } = string.Empty;
    [JsonPropertyName("isOpen")]        public int IsOpen { get; set; }
}
