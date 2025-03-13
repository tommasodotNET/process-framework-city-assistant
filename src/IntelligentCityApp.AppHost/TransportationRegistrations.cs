using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace IntelligentCityApp.AppHost;

public static class TransportationRegistrations
{
    public static IDistributedApplicationBuilder RegisterTransportation(this IDistributedApplicationBuilder builder, IResourceBuilder<Resource> sqlInstance)
    {
        IResourceBuilder<IResourceWithConnectionString> undergroundDb;
        if (builder.Environment.IsProduction())
            undergroundDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("TransportUndergroundDB");
        else
            undergroundDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("TransportUndergroundDB");

        var undergroundApi = builder.AddProject<Projects.IntelligentCityApp_Transportation_Underground_API>("intelligentcityapp-transportation-underground-api")
               .WithReference(undergroundDb)
               .WaitFor(sqlInstance);

        var transportationAgent = builder.AddProject<Projects.IntelligentCityApp_Transportation_Agent>("intelligentcityapp-transportation-agent")
            .WaitFor(undergroundApi);

        return builder;
    }
}
