using Microsoft.EntityFrameworkCore;

namespace IntelligentCityApp.Events.Cinemas.API.Entities;

public class CinemaContext : DbContext
{
    public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
    {
    }
    public DbSet<Cinema> Cinemas { get; set; }

}