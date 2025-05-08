namespace IntelligentCityApp.Accomodation.Agent;

public class RentalAPIHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetRentalsAsync()
    {
        return await _httpClient.GetFromJsonAsync<string>("/rental");
    }
}
