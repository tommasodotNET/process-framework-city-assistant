namespace IntelligentCityApp.Events.Festival.API.Entities;

public class Festival
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; } = default!;
    public bool IsOnline { get; set; }
    public decimal? TicketPrice { get; set; }
    public string? Sponsor { get; set; }
    public int? MaxAttendees { get; set; }
}
