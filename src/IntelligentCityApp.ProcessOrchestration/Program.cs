using Azure.AI.OpenAI;
using IntelligentCityApp.ProcessOrchestration;
using IntelligentCityApp.ProcessOrchestration.AgentsConnectors;
using IntelligentCityApp.ProcessOrchestration.Events;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddAzureOpenAIClient("azureOpenAI");
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<AccomodationAgentHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://accomodationAgent");
});
builder.Services.AddHttpClient<EventAgentHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://eventAgent");
});
builder.Services.AddKernel()
    .AddAzureOpenAIChatCompletion("gpt-4o")
    .Services
        .AddSingleton<AccomodationAgentHttpClient>()
        .AddSingleton<EventAgentHttpClient>();
builder.Services.AddSingleton(builder => {
    var processBuilder = new ProcessBuilder("CityAgentsOrchestration")
        .AddIntelligentCityProcessStepsAndFlows();
    return processBuilder;
});
builder.Services.AddTransient(builder => {
    var processBuilder = builder.GetRequiredService<ProcessBuilder>();
    return processBuilder.Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/city-agents-orchestration", async (KernelProcess process, Kernel kernel, string userRequest) =>
{
    await using var runningProcess = await process.StartAsync(
        kernel,
        new KernelProcessEvent { Id = IntelligentCityEvents.NewRequestReceived, Data = userRequest }
    );

    return Results.Ok();
})
.WithName("CityAgentsOrchestration");

app.MapDefaultEndpoints();

app.Run();