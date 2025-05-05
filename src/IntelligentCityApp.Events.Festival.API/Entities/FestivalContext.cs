using Microsoft.EntityFrameworkCore;

namespace IntelligentCityApp.Events.Festival.API.Entities;

public class FestivalContext : DbContext
{
    public FestivalContext(DbContextOptions<FestivalContext> options) : base(options)
    {
    }
    public DbSet<Festival> Festivals { get; set; }
}
