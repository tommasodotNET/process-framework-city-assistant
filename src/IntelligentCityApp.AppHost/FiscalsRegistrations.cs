using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace IntelligentCityApp.AppHost;

public static class FiscalsRegistrations
{
    public static IDistributedApplicationBuilder RegisterFiscals(this IDistributedApplicationBuilder builder, IResourceBuilder<Resource> sqlInstance)
    {
        IResourceBuilder<IResourceWithConnectionString> fiscalDb;
        if (builder.Environment.IsProduction())
        {
            fiscalDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("FiscalDB");
        }
        else
        {
            fiscalDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("FiscalDB");
        }

        var fiscalApi = builder.AddProject<Projects.IntelligentCityApp_Fiscals_API>("intelligentcityapp-fiscals-api")
               .WithReference(fiscalDb)
               .WaitFor(sqlInstance);

        var fiscalAgent = builder.AddProject<Projects.IntelligentCityApp_Fiscals_Agent>("intelligentcityapp-fiscals-agent")
            .WaitFor(fiscalApi);

        return builder;
    }
}
