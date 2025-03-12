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

builder.AddProject<Projects.IntelligentCityApp_PublicUtilities_Agent>("intelligentcityapp-publicutilities-agent");
builder.AddProject<Projects.IntelligentCityApp_PublicUtilities_API>("intelligentcityapp-publicutilities-api");

builder.AddProject<Projects.IntelligentCityApp_Fiscals_Agent>("intelligentcityapp-fiscals-agent");
builder.AddProject<Projects.IntelligentCityApp_Fiscals_API>("intelligentcityapp-fiscals-api");

builder.AddProject<Projects.IntelligentCityApp_Accomodation_Agent>("intelligentcityapp-accomodation-agent");
builder.AddProject<Projects.IntelligentCityApp_Accomodation_Hotels_API>("intelligentcityapp-hotels-api");
builder.AddProject<Projects.IntelligentCityApp_Accomodation_Rental_API>("intelligentcityapp-rental-api");
builder.AddProject<Projects.IntelligentCityApp_Accomodation_Parking_API>("intelligentcityapp-parking-api");

builder.Build().Run();
