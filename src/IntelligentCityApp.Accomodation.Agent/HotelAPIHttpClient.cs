using System;

namespace IntelligentCityApp.Accomodation.Agent;

public class HotelAPIHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetHotelsAsync()
    {
        var response = await _httpClient.GetAsync("/accomodations");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
