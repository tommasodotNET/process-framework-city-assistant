using Microsoft.EntityFrameworkCore;

namespace IntelligentCityApp.Accomodation.Hotels.API.Entities;

public class AccomodationContext : DbContext
{
    public AccomodationContext(DbContextOptions<AccomodationContext> options) : base(options)
    {
    }

    public DbSet<Accomodation> Accomodations { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}
