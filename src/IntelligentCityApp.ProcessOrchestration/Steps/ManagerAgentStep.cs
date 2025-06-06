using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        var responseFormat = JsonSchema.FromType<IntentResult>().ToJson();
        var responseFormatJson = ChatResponseFormat.CreateJsonSchemaFormat(
            jsonSchemaFormatName: "intent_result",
            jsonSchema: BinaryData.FromString(responseFormat),
            jsonSchemaIsStrict: true);

        ChatMessageContent response = await service.GetChatMessageContentAsync(localHistory, new OpenAIPromptExecutionSettings { ResponseFormat = responseFormatJson });
        IntentResult intent = JsonSerializer.Deserialize<IntentResult>(response.ToString())!;

        var sharedChatHistory = new ChatHistory();
        sharedChatHistory.Add(new ChatMessageContent(AuthorRole.User, userRequest));

        var userIntent = Enum.Parse<UserIntents>(intent.UserIntent, ignoreCase: true);

        switch (userIntent)
        {
            case UserIntents.RetrieveAccomodation:
                await context.EmitEventAsync(new() { Id = IntelligentCityEvents.RetrieveAccomodation, Data = sharedChatHistory });
                break;
            case UserIntents.RetrieveEvents:
                await context.EmitEventAsync(new() { Id = IntelligentCityEvents.RetrieveEvents, Data = sharedChatHistory });
                break;
        }
    }

    public enum UserIntents
    {
        RetrieveAccomodation,
        RetrieveEvents
    }

    [DisplayName("IntentResult")]
    [Description("this is the result description")]
    public sealed record IntentResult(
        [property:Description("Represents the intent identified by the agent. The value is one of the available intents.")]
        [property:Required(AllowEmptyStrings = true)]
        string UserIntent,
        [property:Description("Rationale for the value assigned to IsRequestingUserInput")]
        [property:Required(AllowEmptyStrings = true)]
        string Rationale);
}