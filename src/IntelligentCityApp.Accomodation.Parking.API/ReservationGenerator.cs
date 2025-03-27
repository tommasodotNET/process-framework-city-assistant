using Bogus;

namespace IntelligentCityApp.Accomodation.Parking.API;

public static class ReservationGenerator
{
    public static List<Entities.Parking> GenerateParkings()
    {
        var faker = new Faker("it");
        var parkings = new List<Entities.Parking>();

        for (int i = 0; i < 10; i++)
        {
            var parking = new Entities.Parking
            {
                Id = Guid.NewGuid(),
                Cost = faker.Finance.Amount(5, 20),
                IsAvailableDaily = faker.Random.Bool(),
                PaymentMethod = faker.PickRandom(new[] { "Cash", "Credit Card", "Mobile Payment" }),
                MapLocation = new Entities.MapLocation
                {
                    Latitude = faker.Address.Latitude(45.4642, 45.4842), // Coordinates for Milan
                    Longitude = faker.Address.Longitude(9.1899, 9.2099) // Coordinates for Milan
                },
                UpcomingReservations = GenerateReservations(faker),
                TotalSpots = faker.Random.Int(50, 500),
                AvailableSpots = faker.Random.Int(0, 50)
            };

            parkings.Add(parking);
        }

        return parkings;
    }

    private static List<Entities.Reservation> GenerateReservations(Faker faker)
    {
        var reservations = new List<Entities.Reservation>();

        for (int i = 0; i < faker.Random.Int(1, 500); i++)
        {
            var reservation = new Entities.Reservation
            {
                Id = Guid.NewGuid(),
                StartTime = faker.Date.Future(),
                EndTime = faker.Date.Future(),
                ReservedBy = faker.Name.FullName(),
                IsPaid = faker.Random.Bool(),
                CarLicensePlate = faker.Vehicle.Vin()
            };

            reservations.Add(reservation);
        }

        return reservations;
    }
}
