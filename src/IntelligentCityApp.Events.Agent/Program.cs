using IntelligentCityApp.Events.Agent;
using IntelligentCityApp.Events.Agent.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

builder.AddServiceDefaults();
builder.AddAzureOpenAIClient("azureOpenAI");
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<CinemaAPIHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://intelligentcityapp-events-cinemas-api");
});
builder.Services.AddHttpClient<FestivalAPIHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://intelligentcityapp-events-festival-api");
});
builder.Services.AddSingleton<EventPlugin>();
builder.Services.AddKernel().AddAzureOpenAIChatCompletion("gpt-4o");
builder.Services.AddSingleton(builder =>
{
    var _settings = new OpenAIPromptExecutionSettings()
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
        Temperature = 0.1,
        MaxTokens = 500,
    };
    var agent = new ChatCompletionAgent
    {
        Name = "EventAgent",
        Instructions = "You are a helpful assistant. Answer the user's questions to the best of your ability using your tools. Please, get markdown response",
        Kernel = builder.GetRequiredService<Kernel>(),
        Arguments = new(_settings)
    };
    agent.Kernel.Plugins.AddFromObject(builder.GetRequiredService<EventPlugin>());

    return agent;
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/agents/events", async (ChatCompletionAgent agent, ChatHistory history) =>
{
    var agentThread = new ChatHistoryAgentThread();

    await foreach (var response in agent.InvokeAsync(history, agentThread))
    {
        return response.Message.Content;
    }

    return null;
})
.WithName("GetEvents");

app.Run();