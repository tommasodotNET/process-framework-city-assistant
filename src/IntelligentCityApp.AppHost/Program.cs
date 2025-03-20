using IntelligentCityApp.AppHost;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var openai = builder.AddConnectionString("openAIConnection");

IResourceBuilder<Resource> sqlInstance;
if (builder.Environment.IsProduction())
    sqlInstance = builder.AddAzureSqlServer("IntelligentCityServer");
else
    sqlInstance = builder.AddSqlServer("IntelligentCityServer")
        .WithLifetime(ContainerLifetime.Persistent);

builder.RegisterTransportation(sqlInstance);
builder.RegisterEvents(sqlInstance);
builder.RegisterPublicUtilities(sqlInstance);
builder.RegisterFiscals(sqlInstance);
var accomodationAgent = builder.RegisterAccomodation(sqlInstance);

builder.AddProject<Projects.IntelligentCityApp_ProcessOrchestration>("process-orchestrator")
    .WithReference(accomodationAgent);

builder.Build().Run();
