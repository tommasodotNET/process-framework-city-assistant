using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace IntelligentCityApp.AppHost;

public static class EventRegistrations
{
    public static IDistributedApplicationBuilder RegisterEvents(this IDistributedApplicationBuilder builder, IResourceBuilder<Resource> sqlInstance)
    {
        builder.AddProject<Projects.IntelligentCityApp_Events_Agent>("intelligentcityapp-events-agent");
        builder.AddProject<Projects.IntelligentCityApp_Events_Cinemas_API>("intelligentcityapp-events-cinemas-api");
        builder.AddProject<Projects.IntelligentCityApp_Events_Municipalities_API>("intelligentcityapp-events-municipalities-api");
        builder.AddProject<Projects.IntelligentCityApp_Events_Festival_API>("intelligentcityapp-events-festival-api");
        builder.AddProject<Projects.IntelligentCityApp_Events_Tickets_API>("intelligentcityapp-events-tickets-api");

        IResourceBuilder<IResourceWithConnectionString> cinemaDb;
        IResourceBuilder<IResourceWithConnectionString> municipalitiesDb;
        IResourceBuilder<IResourceWithConnectionString> festivalDb;
        IResourceBuilder<IResourceWithConnectionString> ticketsDb;
        if (builder.Environment.IsProduction())
        {
            cinemaDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("CinemaDB");
            municipalitiesDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("MunicipalitiesDB");
            festivalDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("FestivalDB");
            ticketsDb = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("TicketsDB");
        }
        else
        {
            cinemaDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("CinemaDB");
            municipalitiesDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("MunicipalitiesDB");
            festivalDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("FestivalDB");
            ticketsDb = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("TicketsDB");
        }

        builder.AddProject<Projects.IntelligentCityApp_Events_Cinemas_API>("intelligentcityapp-events-cinemas-api")
            .WithReference(cinemaDb);

        builder.AddProject<Projects.IntelligentCityApp_Events_Municipalities_API>("intelligentcityapp-events-municipalities-api")
            .WithReference(municipalitiesDb);

        builder.AddProject<Projects.IntelligentCityApp_Events_Festival_API>("intelligentcityapp-events-festival-api")
            .WithReference(festivalDb);

        builder.AddProject<Projects.IntelligentCityApp_Events_Tickets_API>("intelligentcityapp-events-tickets-api")
            .WithReference(ticketsDb);

        return builder;
    }
}
