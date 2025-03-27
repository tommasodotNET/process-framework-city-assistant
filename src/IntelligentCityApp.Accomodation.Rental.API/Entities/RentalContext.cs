using Microsoft.EntityFrameworkCore;

namespace IntelligentCityApp.Accomodation.Rental.API.Entities;

public class RentalContext : DbContext
{
    public DbSet<RentalEntity> Rentals { get; set; }

    public DbSet<Vehicle> Vehicles { get; set; }

    public RentalContext(DbContextOptions<RentalContext> options) : base(options)
    {
    }
}
