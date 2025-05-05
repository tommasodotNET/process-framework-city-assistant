using Bogus;

namespace IntelligentCityApp.Events.Festival.API;

public static class ReservationGenerator
{
    public static List<Entities.Festival> GenerateFestivals()
    {
        var festivalFaker = new Faker<Entities.Festival>("it")
            .RuleFor(f => f.Id, _ => Guid.NewGuid())
            .RuleFor(f => f.Name, f => f.Lorem.Word() + " Festival")
            .RuleFor(f => f.Description, f => f.Lorem.Paragraph())
            .RuleFor(f => f.Category, f => f.Commerce.Department())
            .RuleFor(f => f.Location, f => f.Address.City())
            .RuleFor(f => f.IsOnline, f => f.Random.Bool())
            .RuleFor(f => f.TicketPrice, f => f.Random.Decimal(5, 500))
            .RuleFor(f => f.Sponsor, f => f.Company.CompanyName())
            .RuleFor(f => f.MaxAttendees, f => f.Random.Int(100, 1000))
            .RuleFor(f => f.StartDate, f => f.Date.Future(1))
            .RuleFor(f => f.EndDate, (f, festival) => festival.StartDate.AddDays(f.Random.Int(1, 5)));

        return festivalFaker.Generate(20);
    }
}
