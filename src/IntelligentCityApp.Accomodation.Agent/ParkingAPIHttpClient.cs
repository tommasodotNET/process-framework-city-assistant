using System;

namespace IntelligentCityApp.Accomodation.Agent;

public class ParkingAPIHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetParkingAsync()
    {
        var response = await _httpClient.GetAsync("/parkings");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
