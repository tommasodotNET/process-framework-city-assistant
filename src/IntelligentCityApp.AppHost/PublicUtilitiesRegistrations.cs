using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace IntelligentCityApp.AppHost;

public static class PublicUtilitiesRegistrations
{
    public static IDistributedApplicationBuilder RegisterPublicUtilities(this IDistributedApplicationBuilder builder, IResourceBuilder<Resource> sqlInstance)
    {
        IResourceBuilder<IResourceWithConnectionString> publicDb;
        if (builder.Environment.IsProduction())
        {
            publicDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("PublicDB");
        }
        else
        {
            publicDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("PublicDB");
        }


        var publicApi = builder.AddProject<Projects.IntelligentCityApp_PublicUtilities_API>("intelligentcityapp-publicutilities-api")
               .WithReference(publicDb)
               .WaitFor(sqlInstance);

        var publicAgent = builder.AddProject<Projects.IntelligentCityApp_PublicUtilities_Agent>("intelligentcityapp-publicutilities-agent")
            .WaitFor(publicApi);

        return builder;
    }
}
