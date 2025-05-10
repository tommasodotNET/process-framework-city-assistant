using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;
using System.Text.Json;

namespace IntelligentCityApp.ProcessOrchestration.AgentsConnectors;

public class AccomodationAgentHttpClient(HttpClient httpClient)
{
    public async Task<Message> RetrieveAccomodationAsync(ChatHistory chatHistory)
    {
        var response = await httpClient.PostAsync("/agents/accomodation", new StringContent(JsonSerializer.Serialize(chatHistory), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Message>();
    }
}

public record Message(string Text);