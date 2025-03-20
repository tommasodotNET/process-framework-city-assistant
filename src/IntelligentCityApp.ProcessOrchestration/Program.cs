using Azure.AI.OpenAI;
using IntelligentCityApp.ProcessOrchestration;
using IntelligentCityApp.ProcessOrchestration.AgentsConnectors;
using IntelligentCityApp.ProcessOrchestration.Events;
using Microsoft.SemanticKernel;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var otelExporterEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
var otelExporterHeaders = builder.Configuration["OTEL_EXPORTER_OTLP_HEADERS"];

AppContext.SetSwitch("Microsoft.SemanticKernel.Experimental.GenAI.EnableOTelDiagnosticsSensitive", true);

var loggerFactory = LoggerFactory.Create(builder =>
{
    // Add OpenTelemetry as a logging provider
    builder.AddOpenTelemetry(options =>
    {
        options.AddOtlpExporter(exporter => {exporter.Endpoint = new Uri(otelExporterEndpoint); exporter.Headers = otelExporterHeaders; exporter.Protocol = OtlpExportProtocol.Grpc;});
        // Format log messages. This defaults to false.
        options.IncludeFormattedMessage = true;
    });

    builder.AddTraceSource("Microsoft.SemanticKernel");
    builder.SetMinimumLevel(LogLevel.Information);
});

using var traceProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("Microsoft.SemanticKernel*")
    .AddOtlpExporter(exporter => {exporter.Endpoint = new Uri(otelExporterEndpoint); exporter.Headers = otelExporterHeaders; exporter.Protocol = OtlpExportProtocol.Grpc;})
    .Build();

using var meterProvider = Sdk.CreateMeterProviderBuilder()
    .AddMeter("Microsoft.SemanticKernel*")
    .AddOtlpExporter(exporter => {exporter.Endpoint = new Uri(otelExporterEndpoint); exporter.Headers = otelExporterHeaders; exporter.Protocol = OtlpExportProtocol.Grpc;})
    .Build();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.AddAzureOpenAIClient("openAIConnection");
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<AccomodationAgentHttpClient>(client =>
{
    client.BaseAddress = new Uri("https+http://accomodation-agent");
});
builder.Services.AddSingleton(builder => {
    var kernelBuilder = Kernel.CreateBuilder();
    kernelBuilder.AddAzureOpenAIChatCompletion("gpt-4o", builder.GetService<AzureOpenAIClient>());
    kernelBuilder.Services.AddSingleton(builder.GetRequiredService<AccomodationAgentHttpClient>());
    return kernelBuilder.Build();
});
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
    using var runningProcess = await process.StartAsync(
        kernel,
        new KernelProcessEvent { Id = IntelligentCityEvents.NewRequestReceived, Data = userRequest }
    );

    return Results.Ok();
})
.WithName("CityAgentsOrchestration");

app.MapDefaultEndpoints();

app.Run();