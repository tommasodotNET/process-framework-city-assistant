using Bogus;
using IntelligentCityApp.Accomodation.Hotels.API.Entities;

namespace IntelligentCityApp.Accomodation.Hotels.API;

public static class ReservationGenerator
{
    public static List<string> RoomTypes = new() { "Regular", "Deluxe", "Superior", "Suite", "King Suite", "Junior Suite" };

    public static List<Entities.Accomodation> GenerateAccomodations()
    {
        var faker = new Faker("it");
        var accomodations = new List<Entities.Accomodation>();

        for (int i = 0; i < faker.Random.Int(15, 100); i++)
        {
            var accomodation = new Entities.Accomodation
            {
                //Id = i + 1,
                Name = faker.Company.CompanyName(),
                Address = faker.Address.FullAddress(),
                Rooms = GenerateRooms(faker)
            };

            accomodations.Add(accomodation);
        }

        return accomodations;
    }

    private static List<Room> GenerateRooms(Faker faker)
    {
        var rooms = new List<Room>();
        int numberOfRooms = faker.Random.Int(5, 20);

        for (int i = 1; i <= numberOfRooms; i++)
        {
            var id = faker.Random.Guid();
            var room = new Room
            {
                Id = id,
                Type = $"Type {faker.PickRandom(RoomTypes)}",
                NumberOfBeds = faker.Random.Int(1, 3),
                IsAvailable = faker.Random.Bool(),
                Reservations = GenerateReservations(faker, id)
            };

            rooms.Add(room);
        }

        return rooms;
    }

    private static List<Reservation> GenerateReservations(Faker faker, Guid roomId)
    {
        var reservations = new List<Reservation>();
        int numberOfReservations = faker.Random.Int(1, 40);

        for (int i = 1; i <= numberOfReservations; i++)
        {
            var reservation = new Reservation
            {
                Id = faker.Random.Guid(),
                RoomId = roomId,
                CheckInDate = DateOnly.FromDateTime(faker.Date.Between(new DateTime(2025, 4, 1), new DateTime(2025, 6, 30))),
                CheckOutDate = DateOnly.FromDateTime(faker.Date.Between(new DateTime(2025, 4, 2), new DateTime(2025, 7, 1))),
                GuestName = faker.Name.FullName(),
                GuestNationality = faker.Address.Country()
            };

            reservations.Add(reservation);
        }

        return reservations;
    }
}
