namespace IntelligentCityApp.Accomodation.Agent;

public class ParkingAPIHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetParkingAsync()
    {
        return await _httpClient.GetFromJsonAsync<string>("/parkings");
    }
}
