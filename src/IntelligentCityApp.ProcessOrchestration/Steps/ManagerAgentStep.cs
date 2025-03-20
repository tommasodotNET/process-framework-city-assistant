using System.ComponentModel;
using System.Text.Json;
using IntelligentCityApp.ProcessOrchestration.Events;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using NJsonSchema;
using ChatResponseFormat = OpenAI.Chat.ChatResponseFormat;

namespace IntelligentCityApp.ProcessOrchestration.Steps;

public class ManagerAgentStep : KernelProcessStep
{
    public static class Functions
    {
        public const string IdentifyUserRequest = nameof(IdentifyUserRequest);
        public const string AgentResponded = nameof(AgentResponded);
    }

    [KernelFunction(Functions.IdentifyUserRequest)]
    public async Task IdentifyUserRequestAsync(KernelProcessStepContext context, Kernel kernel, string userRequest)
    {
        // Dynamically retrieve the available intents from the UserRequests class
        var availableIntents = Enum.GetValues(typeof(UserIntents))
            .Cast<UserIntents>()
            .Select(intent => intent.ToString())
            .ToArray();

        ChatHistory localHistory =
        [
            new ChatMessageContent(AuthorRole.System, $"You need to identify the user request and return an intent name. The available intents are {JsonSerializer.Serialize(availableIntents)}"),
            new ChatMessageContent(AuthorRole.User, userRequest)
        ];

        IChatCompletionService service = kernel.GetRequiredService<IChatCompletionService>();

        ChatMessageContent response = await service.GetChatMessageContentAsync(localHistory, new OpenAIPromptExecutionSettings { ResponseFormat = s_intentResponseFormat });
        IntentResult intent = JsonSerializer.Deserialize<IntentResult>(response.ToString())!;

        var sharedChatHistory = new ChatHistory();
        sharedChatHistory.Add(new ChatMessageContent(AuthorRole.User, userRequest));

        switch (intent.UserIntent)
        {
            case UserIntents.RetrieveAccomodation:
                await context.EmitEventAsync(new() { Id = IntelligentCityEvents.RetrieveAccomodation, Data = sharedChatHistory });
                break;
        }
    }

    // Here we should have another function to handle the other intents. When an agent answers, we should receive an event and reanalyze the user request to see if we need
    // to invoke another agent or if we can finish the process. All the previous agents responses should be provided to the next agent.
    [KernelFunction(Functions.AgentResponded)]
    public async Task AgentRespondedAsync(KernelProcessStepContext context, Kernel kernel, string userRequest)
    {
        // Here we should have another function to handle the other intents. When an agent answers, we should receive an event and reanalyze the user request to see if we need
        // to invoke another agent or if we can finish the process. All the previous agents responses should be provided to the next agent.
        await context.EmitEventAsync(new() { Id = IntelligentCityEvents.FinalizeProcess, Data = userRequest });
    }

    public enum UserIntents
    {
        RetrieveAccomodation,
        RetrieveSummary,
        RetrieveTouristAttractions,
        RetrieveWeather
    }

    private static readonly ChatResponseFormat s_intentResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
        jsonSchemaFormatName: "intent_result",
        jsonSchema: BinaryData.FromString(JsonSchema.FromType<IntentResult>().ToString()!),
        jsonSchemaIsStrict: true);

    [DisplayName("IntentResult")]
    [Description("this is the result description")]
    public sealed record IntentResult(
        [property:Description("Represents the intent identified by the agent. The value is one of the available intents.")]
        UserIntents UserIntent,
        [property:Description("Rationale for the value assigned to IsRequestingUserInput")]
        string Rationale);
}
