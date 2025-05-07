using System;

namespace IntelligentCityApp.Accomodation.Agent;

public class RentalAPIHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetRentalsAsync()
    {
        var response = await _httpClient.GetAsync("/rental");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
