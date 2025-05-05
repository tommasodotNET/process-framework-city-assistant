using Bogus;

namespace IntelligentCityApp.Events.Cinemas.API;

public static class ReservationGenerator
{
    public static List<Entities.Cinema> GenerateCinemasFromThePast()
    {
        var faker = new Faker("it");
        var cinemas = new List<Entities.Cinema>();

        for (int i = 0; i < 20; i++)
        {
            var cinema = new Entities.Cinema
            {
                Id = Guid.NewGuid(),
                Name = faker.Company.CompanyName(),
                Year = faker.Date.Past(30).Year,
                Actors = GenerateActors(),
                Authors = GenerateAuthors(),
                ProductionCompany = GenerateProductionCompany()
            };
            cinemas.Add(cinema);
        }

        return cinemas;
    }

    private static List<Entities.Actor> GenerateActors()
    {
        var faker = new Faker<Entities.Actor>("it")
            .RuleFor(a => a.Surname, f => f.Name.LastName())
            .RuleFor(a => a.Name, f => f.Name.FirstName())
            .RuleFor(a => a.Name, f => f.Name.FullName());

        return faker.Generate(3);
    }

    private static List<Entities.Author> GenerateAuthors()
    {
        var faker = new Faker<Entities.Author>("it")
            .RuleFor(a => a.Id, _ => Guid.NewGuid())
            .RuleFor(a => a.Surname, f => f.Name.LastName())
            .RuleFor(a => a.Name, f => f.Name.FirstName());

        return faker.Generate(2);
    }

    private static Entities.ProductionCompany GenerateProductionCompany()
    {
        var faker = new Faker<Entities.ProductionCompany>("it")
            .RuleFor(p => p.Country, f => f.Locale)
            .RuleFor(p => p.Name, f => f.Company.CompanyName());

        return faker.Generate();
    }
}
