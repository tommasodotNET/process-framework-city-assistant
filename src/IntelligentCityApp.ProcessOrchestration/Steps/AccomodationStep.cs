using System;
using IntelligentCityApp.ProcessOrchestration.AgentsConnectors;
using IntelligentCityApp.ProcessOrchestration.Events;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace IntelligentCityApp.ProcessOrchestration.Steps;

public class AccomodationStep : KernelProcessStep
{
    public static class Functions
    {
        public const string RetrieveAccomodation = nameof(RetrieveAccomodation);
    }

    [KernelFunction(Functions.RetrieveAccomodation)]
    public async Task RetrieveAccomodationAsync(KernelProcessStepContext context, Kernel kernel, ChatHistory chatHistory)
    {
        var accomodationAgentHttpClient = kernel.GetRequiredService<AccomodationAgentHttpClient>();
        var accomodationAgentResponse = await accomodationAgentHttpClient.RetrieveAccomodationAsync(chatHistory);
        Console.WriteLine($"Accomodation retrieved: {accomodationAgentResponse}");
        chatHistory.Add(new ChatMessageContent(AuthorRole.Assistant, accomodationAgentResponse));
        await context.EmitEventAsync(new () { Id = IntelligentCityEvents.InformationRetrieved, Data = chatHistory });
    }
}