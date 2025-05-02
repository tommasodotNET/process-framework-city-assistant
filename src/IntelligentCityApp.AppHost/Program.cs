using IntelligentCityApp.AppHost;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var openai = builder.AddConnectionString("azureOpenAI");

IResourceBuilder<Resource> sqlInstance;
if (builder.Environment.IsProduction())
    sqlInstance = builder.AddAzureSqlServer("IntelligentCityServer");
else
    sqlInstance = builder.AddSqlServer("IntelligentCityServer")
        .WithLifetime(ContainerLifetime.Persistent);

var accomodationAgent = builder.RegisterAccomodation(sqlInstance);
var eventAgent = builder.RegisterEvents(sqlInstance);

var process = builder.AddProject<Projects.IntelligentCityApp_ProcessOrchestration>("process-orchestrator")
    .WithReference(openai)
    .WithReference(accomodationAgent)
    .WithReference(eventAgent)
    .WaitFor(accomodationAgent)
    .WaitFor(eventAgent);

builder.AddProject<Projects.IntelligentCityApp_WebClient>("intelligentcityapp-webclient")
    .WithReference(process)
    .WaitFor(process);

builder.Build().Run();
