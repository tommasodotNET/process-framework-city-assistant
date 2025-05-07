using IntelligentCityApp.Accomodation.Agent;
using IntelligentCityApp.Accomodation.Agent.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

builder.AddServiceDefaults();
builder.AddAzureOpenAIClient("azureOpenAI");
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<HotelAPIHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://intelligentcityapp-accomodation-hotels-api");
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddHttpClient<ParkingAPIHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://intelligentcityapp-accomodation-parking-api");
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddHttpClient<RentalAPIHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://intelligentcityapp-accomodation-rental-api");
    client.Timeout = TimeSpan.FromSeconds(60);
});
builder.Services.AddSingleton<AccomodationPlugin>();
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
        Name = "AccomodationAgent",
        Instructions = "You are a helpful assistant. Answer the user's questions to the best of your ability using your tools. Please, get markdown response",
        Kernel = builder.GetRequiredService<Kernel>(),
        Arguments = new(_settings)
    };
    agent.Kernel.Plugins.AddFromObject(builder.GetRequiredService<AccomodationPlugin>());

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

app.MapPost("/agents/accomodation", async (ChatCompletionAgent agent, ChatHistory history) =>
{
    var agentThread = new ChatHistoryAgentThread();

    await foreach (var response in agent.InvokeAsync(history, agentThread))
    {
        return response.Message.Content;
    }

    return null;
})
.WithName("GetAccomodation");

app.Run();