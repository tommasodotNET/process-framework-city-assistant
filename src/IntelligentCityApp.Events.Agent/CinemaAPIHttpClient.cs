using System;

namespace IntelligentCityApp.Events.Agent;

public class CinemaAPIHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetCinemaAsync()
    {
        var response = await _httpClient.GetAsync("/cinemas");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
