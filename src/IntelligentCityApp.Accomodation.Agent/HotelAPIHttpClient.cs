namespace IntelligentCityApp.Accomodation.Agent;

public class HotelAPIHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetHotelsAsync(DateTime userRequestDate)
    {
        var response = await _httpClient.GetAsync($"/accomodations/?searchDate={userRequestDate:yyyy-MM-dd}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
