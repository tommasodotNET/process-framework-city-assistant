namespace IntelligentCityApp.Accomodation.Hotels.API.Entities;

public class Accomodation
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required List<Room> Rooms { get; set; }
}

public class Room
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public int NumberOfBeds { get; set; }

    public bool IsAvailable { get; set; }

    public List<Reservation> Reservations { get; set; } = new();

    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public required string GuestName { get; set; }
        public required string GuestNationality { get; set; }
    }
}
