using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace IntelligentCityApp.AppHost;

public static class EventRegistrations
{
    public static IResourceBuilder<ProjectResource> RegisterEvents(this IDistributedApplicationBuilder builder, IResourceBuilder<Resource> sqlInstance)
    {
        IResourceBuilder<IResourceWithConnectionString> cinemaDb;
        IResourceBuilder<IResourceWithConnectionString> festivalDb;
        if (builder.Environment.IsProduction())
        {
            cinemaDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("CinemaDB");
            festivalDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("FestivalDB");
        }
        else
        {
            cinemaDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("CinemaDB");
            festivalDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("FestivalDB");
        }

        var cinemaApi = builder.AddProject<Projects.IntelligentCityApp_Events_Cinemas_API>("intelligentcityapp-events-cinemas-api")
            .WithReference(cinemaDb)
            .WaitFor(sqlInstance);

        var festivalApi = builder.AddProject<Projects.IntelligentCityApp_Events_Festival_API>("intelligentcityapp-events-festival-api")
            .WithReference(festivalDb)
            .WaitFor(sqlInstance);

        var eventAgent = builder.AddProject<Projects.IntelligentCityApp_Events_Agent>("intelligentcityapp-events-agent")
            .WaitFor(cinemaApi)
            .WaitFor(festivalApi);

        return eventAgent;
    }
}
