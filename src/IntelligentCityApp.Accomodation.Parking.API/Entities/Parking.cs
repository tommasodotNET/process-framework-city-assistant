using Microsoft.EntityFrameworkCore;

namespace IntelligentCityApp.Accomodation.Parking.API.Entities;

public class Parking
{
    public Guid Id { get; set; }
    public decimal Cost { get; set; }
    public bool IsAvailableDaily { get; set; }
    public required string PaymentMethod { get; set; }
    public required MapLocation MapLocation { get; set; }
    public List<Reservation> UpcomingReservations { get; set; } = new List<Reservation>();
    public int TotalSpots { get; set; }
    public int AvailableSpots { get; set; }
}

[PrimaryKey(nameof(Latitude), nameof(Longitude))]
public class MapLocation
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class Reservation
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public required string ReservedBy { get; set; }
    public bool IsPaid { get; set; }
    public required string CarLicensePlate { get; set; }
}
