using System;
using IntelligentCityApp.ProcessOrchestration.AgentsConnectors;
using IntelligentCityApp.ProcessOrchestration.Events;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace IntelligentCityApp.ProcessOrchestration.Steps;

public class EventStep : KernelProcessStep
{
    public static class Functions
    {
        public const string RetrieveEvents = nameof(RetrieveEvents);
    }

    [KernelFunction(Functions.RetrieveEvents)]
    public async Task RetrieveeventAsync(KernelProcessStepContext context, Kernel kernel, ChatHistory chatHistory)
    {
        var eventAgentHttpClient = kernel.GetRequiredService<EventAgentHttpClient>();
        var eventAgentResponse = await eventAgentHttpClient.RetrieveEventAsync(chatHistory);
        Console.WriteLine($"event retrieved: {eventAgentResponse}");
        chatHistory.Add(new ChatMessageContent(AuthorRole.Assistant, eventAgentResponse));
        await context.EmitEventAsync(new () { Id = IntelligentCityEvents.InformationRetrieved, Data = chatHistory });
    }
}