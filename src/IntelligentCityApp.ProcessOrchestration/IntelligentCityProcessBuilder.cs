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

        processBuilder
            .OnEvent(IntelligentCityEvents.NewRequestReceived)
            .SendEventTo(new(managerAgentStep, Steps.ManagerAgentStep.Functions.IdentifyUserRequest, parameterName: "userRequest"));

        managerAgentStep
            .OnEvent(IntelligentCityEvents.RetrieveAccomodation)
            .SendEventTo(new(accomodationStep, Steps.AccomodationStep.Functions.RetrieveAccomodation, parameterName: "chatHistory"));

        accomodationStep
            .OnEvent(IntelligentCityEvents.InformationRetrieved)
            .SendEventTo(new(managerAgentStep, Steps.ManagerAgentStep.Functions.AgentResponded, parameterName: "userRequest"));

        managerAgentStep
            .OnEvent(IntelligentCityEvents.FinalizeProcess)
            .StopProcess();

        return processBuilder;
    }
}
