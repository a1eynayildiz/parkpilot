using System.Net.Http.Json;

namespace ParkingPredictor.Collector;

public class IsparkClient
{
    private const string ParksUrl = "https://api.ibb.gov.tr/ispark/Park";
    private readonly HttpClient _httpClient;

    public IsparkClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<IsparkParkDto>> GetParksAsync(CancellationToken cancellationToken = default)
    {
        var parks = await _httpClient.GetFromJsonAsync<List<IsparkParkDto>>(ParksUrl, cancellationToken);
        return parks ?? throw new InvalidOperationException("İSPARK boş yanıt döndürdü.");
    }
}
