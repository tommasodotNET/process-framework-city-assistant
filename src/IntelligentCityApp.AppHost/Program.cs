using IntelligentCityApp.AppHost;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<Resource> sqlInstance;
if (builder.Environment.IsProduction())
    sqlInstance = builder.AddAzureSqlServer("IntelligentCityServer");
else
    sqlInstance = builder.AddSqlServer("IntelligentCityServer");

builder.RegisterTransportation(sqlInstance);
builder.RegisterEvents(sqlInstance);
builder.RegisterPublicUtilities(sqlInstance);
builder.RegisterFiscals(sqlInstance);
builder.RegisterAccomodation(sqlInstance);

builder.Build().Run();
