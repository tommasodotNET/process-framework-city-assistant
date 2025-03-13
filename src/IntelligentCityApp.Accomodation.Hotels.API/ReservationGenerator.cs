using IntelligentCityApp.Accomodation.Hotels.API.Entities;

namespace IntelligentCityApp.Accomodation.Hotels.API;

public static class ReservationGenerator
{
    private static readonly Random random = new();
    private static readonly List<string> hotelNames = new() { "Hotel Excelsior", "Grand Hotel", "Hotel Paradiso", "Hotel Roma", "Hotel Firenze", "Hotel Venezia", "Hotel Torino", "Hotel Napoli", "Hotel Genova", "Hotel Bologna", "Hotel Verona", "Hotel Palermo", "Hotel Catania", "Hotel Bari", "Hotel Trieste" };
    private static readonly List<string> guestNames = new() { "Mario Rossi", "Luigi Bianchi", "Giovanni Verdi", "Francesca Neri", "Anna Russo", "Paolo Ferrari", "Laura Conti", "Marco Gallo", "Elena Greco", "Giorgio Moretti", "Silvia Romano", "Alessandro Costa", "Valentina Fontana", "Roberto Ricci", "Chiara Bruno", "Stefano De Luca", "Federica Rizzo", "Andrea Lombardi", "Sara Esposito", "Davide Marini" };

    public static List<Entities.Accomodation> GenerateAccomodations()
    {
        var accomodations = new List<Entities.Accomodation>();

        for (int i = 0; i < hotelNames.Count; i++)
        {
            var accomodation = new Entities.Accomodation
            {
                Id = i + 1,
                Name = hotelNames[i],
                Address = $"Via {hotelNames[i].Split(' ')[1]} {i + 1}, Milano",
                Rooms = GenerateRooms()
            };

            accomodations.Add(accomodation);
        }

        return accomodations;
    }

    private static List<Room> GenerateRooms()
    {
        var rooms = new List<Room>();
        int numberOfRooms = random.Next(5, 21);

        for (int i = 1; i <= numberOfRooms; i++)
        {
            var room = new Room
            {
                Id = i,
                Type = $"Type {random.Next(1, 5)}",
                NumberOfBeds = random.Next(1, 4),
                IsAvailable = random.Next(0, 2) == 1,
                Reservations = GenerateReservations(i)
            };

            rooms.Add(room);
        }

        return rooms;
    }

    private static List<Room.Reservation> GenerateReservations(int roomId)
    {
        var reservations = new List<Room.Reservation>();
        int numberOfReservations = random.Next(1, 41);

        for (int i = 1; i <= numberOfReservations; i++)
        {
            var reservation = new Room.Reservation
            {
                Id = i,
                RoomId = roomId,
                CheckInDate = GenerateRandomDate(new DateOnly(2025, 4, 1), new DateOnly(2025, 6, 30)),
                CheckOutDate = GenerateRandomDate(new DateOnly(2025, 4, 2), new DateOnly(2025, 7, 1)),
                GuestName = guestNames[random.Next(guestNames.Count)],
                GuestNationality = "Italian"
            };

            reservations.Add(reservation);
        }

        return reservations;
    }

    private static DateOnly GenerateRandomDate(DateOnly start, DateOnly end)
    {
        int range = (end.ToDateTime(TimeOnly.MinValue) - start.ToDateTime(TimeOnly.MinValue)).Days;
        return start.AddDays(random.Next(range));
    }
}
