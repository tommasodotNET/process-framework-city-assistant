using System;
using IntelligentCityApp.ProcessOrchestration.Events;
using Microsoft.SemanticKernel;

namespace IntelligentCityApp.ProcessOrchestration;

public static class IntelligentCityProcessBuilder
{
    public static ProcessBuilder AddIntelligentCityProcessStepsAndFlows(this ProcessBuilder processBuilder)
    {
        var managerAgentStep = processBuilder.AddStepFromType<Steps.ManagerAgentStep>();
        var accomodationStep = processBuilder.AddStepFromType<Steps.AccomodationStep>();
        var eventStep = processBuilder.AddStepFromType<Steps.EventStep>();

        var proxyStep = processBuilder.AddProxyStep([IntelligentCityTopics.AccomodationRetrieved, IntelligentCityTopics.EventsRetrieved]);

        processBuilder
            .OnInputEvent(IntelligentCityEvents.NewRequestReceived)
            .SendEventTo(new(managerAgentStep, Steps.ManagerAgentStep.Functions.IdentifyUserRequest, parameterName: "userRequest"));

        managerAgentStep
            .OnEvent(IntelligentCityEvents.RetrieveAccomodation)
            .SendEventTo(new(accomodationStep, Steps.AccomodationStep.Functions.RetrieveAccomodation, parameterName: "chatHistory"));

        managerAgentStep
            .OnEvent(IntelligentCityEvents.RetrieveEvents)
            .SendEventTo(new(eventStep, Steps.EventStep.Functions.RetrieveEvents, parameterName: "chatHistory"));

        accomodationStep
            .OnEvent(IntelligentCityEvents.InformationRetrieved)
            .EmitExternalEvent(proxyStep, IntelligentCityTopics.AccomodationRetrieved)
            .StopProcess();

        eventStep
            .OnEvent(IntelligentCityEvents.InformationRetrieved)
            .EmitExternalEvent(proxyStep, IntelligentCityTopics.EventsRetrieved)
            .StopProcess();

        return processBuilder;
    }
}

public static class IntelligentCityTopics
{
    public const string AccomodationRetrieved = nameof(AccomodationRetrieved);
    public const string EventsRetrieved = nameof(EventsRetrieved);
}
