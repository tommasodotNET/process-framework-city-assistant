using Bogus;
using IntelligentCityApp.Accomodation.Rental.API.Entities;

namespace IntelligentCityApp.Accomodation.Rental.API;

public static class ReservationGenerator
{
    public static List<RentalEntity> GenerateRentals()
    {
        var vehicleFaker = new Faker<Vehicle>()
            .RuleFor(v => v.Id, f => f.Random.Guid())
            .RuleFor(v => v.VehicleType, f => f.PickRandom<VehicleType>())
            .RuleFor(v => v.CostPerHour, f => f.Finance.Amount(10, 100))
            .RuleFor(v => v.AvailableUnits, f => f.Random.Int(1, 20))
            .RuleFor(v => v.EngineCapacity, f => f.Vehicle.Random.Int(1200, 6000))
            .RuleFor(v => v.InsuranceExpiryDate, f => f.Date.Future());

        var rentalFaker = new Faker<RentalEntity>()
            .RuleFor(r => r.Id, f => f.Random.Guid())
            .RuleFor(r => r.Name, f => f.Company.CompanyName())
            .RuleFor(r => r.Address, f => f.Address.FullAddress())
            .RuleFor(r => r.PricePerNight, f => f.Finance.Amount(50, 500))
            .RuleFor(r => r.MaxOccupancy, f => f.Random.Int(1, 10))
            .RuleFor(r => r.Vehicles, f => vehicleFaker.Generate(f.Random.Int(1, 500)));

        return rentalFaker.Generate(20);
    }
}
