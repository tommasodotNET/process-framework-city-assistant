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
            .WaitFor(sqlInstance)
            .WithHttpCommand("/reset-db", "Reset Database", commandOptions: ResetDb); ;

        var festivalApi = builder.AddProject<Projects.IntelligentCityApp_Events_Festival_API>("intelligentcityapp-events-festival-api")
            .WithReference(festivalDb)
            .WaitFor(sqlInstance)
            .WithHttpCommand("/reset-db", "Reset Database", commandOptions: ResetDb); ;

        var eventAgent = builder.AddProject<Projects.IntelligentCityApp_Events_Agent>("intelligentcityapp-events-agent")
            .WithReference(cinemaApi)
            .WithReference(festivalApi)
            .WaitFor(cinemaApi)
            .WaitFor(festivalApi);

        return eventAgent;
    }

    public static HttpCommandOptions ResetDb = new()
    {
        Description = "Reset the catalog database to its initial state. This will delete and recreate the database.",
        ConfirmationMessage = "Are you sure you want to reset the database?",
        IconName = "DatabaseLightning",
        PrepareRequest = requestContext =>
        {
            return Task.CompletedTask;
        }
    };
}
