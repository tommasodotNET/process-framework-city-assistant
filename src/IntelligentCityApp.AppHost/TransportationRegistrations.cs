using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace IntelligentCityApp.AppHost;

public static class TransportationRegistrations
{
    public static IDistributedApplicationBuilder RegisterTransportation(this IDistributedApplicationBuilder builder, IResourceBuilder<Resource> sqlInstance)
    {
        builder.AddProject<Projects.IntelligentCityApp_Transportation_Agent>("intelligentcityapp-transportation-agent");

        IResourceBuilder<IResourceWithConnectionString> undergroundDb;
        if (builder.Environment.IsProduction())
            undergroundDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("TransportUndergroundDB");
        else
            undergroundDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("TransportUndergroundDB");

        builder.AddProject<Projects.IntelligentCityApp_Transportation_Underground_API>("intelligentcityapp-transportation-underground-api")
               .WithReference(undergroundDb);
        return builder;
    }
}
