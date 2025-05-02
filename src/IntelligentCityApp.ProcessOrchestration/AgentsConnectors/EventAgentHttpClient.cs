using System;
using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel.ChatCompletion;

namespace IntelligentCityApp.ProcessOrchestration.AgentsConnectors;

public class EventAgentHttpClient(HttpClient httpClient)
{
    public async Task<string> RetrieveEventAsync(ChatHistory chatHistory)
    {
        var response = await httpClient.PostAsync("/agents/events", new StringContent(JsonSerializer.Serialize(chatHistory), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }
}
