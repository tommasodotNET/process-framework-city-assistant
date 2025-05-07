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
    public Guid Id { get; set; }
    public required string Type { get; set; }
    public int NumberOfBeds { get; set; }
    public bool IsAvailable(DateTime date) => !Reservations.Any(t => t.CheckInDate >= DateOnly.FromDateTime(date) && t.CheckOutDate <= DateOnly.FromDateTime(date));
    public List<Reservation> Reservations { get; set; } = new();
}

public class Reservation
{
    public Guid Id { get; set; }
    public Guid RoomId { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public required string GuestName { get; set; }
    public required string GuestNationality { get; set; }
}