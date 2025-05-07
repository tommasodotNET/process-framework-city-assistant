using System;

namespace IntelligentCityApp.Events.Agent;

public class FestivalAPIHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetFestivalAsync()
    {
        var response = await _httpClient.GetAsync("/festivals");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
