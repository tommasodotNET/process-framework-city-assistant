using Aspire.Hosting.Azure;
using Microsoft.Extensions.Hosting;

namespace IntelligentCityApp.AppHost;

public static class AccomodationRegistrations
{
    public static IResourceBuilder<ProjectResource> RegisterAccomodation(this IDistributedApplicationBuilder builder, IResourceBuilder<Resource> sqlInstance)
    {
        IResourceBuilder<IResourceWithConnectionString> hotelsDB;
        IResourceBuilder<IResourceWithConnectionString> rentalDB;
        IResourceBuilder<IResourceWithConnectionString> parkingDB;
        if (builder.Environment.IsProduction())
        {
            hotelsDB = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("HotelDb");
            rentalDB = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("RentalDB");
            parkingDB = ((IResourceBuilder<AzureSqlServerResource>)sqlInstance).AddDatabase("ParkingDB");
        }
        else
        {
            hotelsDB = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("HotelDb");
            rentalDB = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("RentalDB");
            parkingDB = ((IResourceBuilder<SqlServerServerResource>)sqlInstance).AddDatabase("ParkingDB");
        }
        var hotelApi = builder.AddProject<Projects.IntelligentCityApp_Accomodation_Hotels_API>("intelligentcityapp-accomodation-hotels-api")
            .WithReference(hotelsDB)
            .WaitFor(sqlInstance)
            .WithHttpCommand("/reset-db", "Reset Database", commandOptions: ResetDb);

        var rentalApi = builder.AddProject<Projects.IntelligentCityApp_Accomodation_Rental_API>("intelligentcityapp-accomodation-rental-api")
            .WithReference(rentalDB)
            .WaitFor(sqlInstance)
            .WithHttpCommand("/reset-db", "Reset Database", commandOptions: ResetDb);

        var parkingApi = builder.AddProject<Projects.IntelligentCityApp_Accomodation_Parking_API>("intelligentcityapp-accomodation-parking-api")
            .WithReference(parkingDB)
            .WaitFor(sqlInstance)
            .WithHttpCommand("/reset-db", "Reset Database", commandOptions: ResetDb);

        var accomodationAgent = builder.AddProject<Projects.IntelligentCityApp_Accomodation_Agent>("intelligentcityapp-accomodation-agent")
            .WithReference(hotelApi)
            .WithReference(rentalApi)
            .WithReference(parkingApi)
            .WaitFor(hotelApi)
            .WaitFor(rentalApi)
            .WaitFor(parkingApi);

        return accomodationAgent;
    }

    public static HttpCommandOptions ResetDb = new()
    {
        Description = "Reset the catalog database to its initial state. This will delete and recreate the database.",
        ConfirmationMessage = "Are you sure you want to reset the hotel database?",
        IconName = "DatabaseLightning",
        PrepareRequest = requestContext =>
        {
            return Task.CompletedTask;
        }
    };
}
