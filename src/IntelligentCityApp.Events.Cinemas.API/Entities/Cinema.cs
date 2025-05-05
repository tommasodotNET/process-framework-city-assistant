namespace IntelligentCityApp.Events.Cinemas.API.Entities;

public class Actor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime BirthDate { get; set; }
}

public class Author
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Surname { get; set; }
}

public class ProductionCompany
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public string? Country { get; set; }
}

public class Cinema
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Year { get; set; }

    public List<Actor> Actors { get; set; } = new();
    public List<Author> Authors { get; set; } = new();
    public ProductionCompany? ProductionCompany { get; set; }
}
