using Microsoft.EntityFrameworkCore;

namespace IntelligentCityApp.Accomodation.Parking.API.Entities;

public class ParkingContext : DbContext
{
    public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
    {
    }
    public DbSet<Parking> Parkings { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

}
