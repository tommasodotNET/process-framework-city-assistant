using Azure.AI.OpenAI;
using IntelligentCityApp.ProcessOrchestration;
using IntelligentCityApp.ProcessOrchestration.AgentsConnectors;
using IntelligentCityApp.ProcessOrchestration.Events;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

builder.AddAzureOpenAIClient("azureOpenAI");
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<AccomodationAgentHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://intelligentcityapp-accomodation-agent");
});
builder.Services.AddHttpClient<EventAgentHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://intelligentcityapp-events-agent");
});
builder.Services.AddSingleton(builder => {
    var kernelBuilder = Kernel.CreateBuilder();
    kernelBuilder.AddAzureOpenAIChatCompletion("gpt-4o", builder.GetService<AzureOpenAIClient>());
    kernelBuilder.Services.AddSingleton(builder.GetRequiredService<AccomodationAgentHttpClient>());
    kernelBuilder.Services.AddSingleton(builder.GetRequiredService<EventAgentHttpClient>());
    return kernelBuilder.Build();
});
builder.Services.AddSingleton(builder => {
    var processBuilder = new ProcessBuilder("CityAgentsOrchestration")
        .AddIntelligentCityProcessStepsAndFlows();
    return processBuilder;
});
builder.Services.AddTransient(builder =>
{
    var processBuilder = builder.GetRequiredService<ProcessBuilder>();
    return processBuilder.Build();
});
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/city-agents-orchestration", async (KernelProcess process, Kernel kernel, string userRequest) =>
{
    var externalMessageChannel = new ExternalEventProxyChannel();
    var runningProcess = await process.StartAsync(
        kernel,
        new KernelProcessEvent { Id = IntelligentCityEvents.NewRequestReceived, Data = userRequest },
        externalMessageChannel
    );

    return Results.Ok();
})
.WithName("CityAgentsOrchestration");

app.MapDefaultEndpoints();

app.MapHub<ProcessFrameworkHub>("/pfevents", options =>
{
    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
});

app.Run();