using IntelligentCityApp.Events.Cinemas.API;
using IntelligentCityApp.Events.Cinemas.API.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.AddSqlServerDbContext<CinemaContext>("CinemaDb");

var app = builder.Build();

//await StartupDatabase(app);

app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Cinema API");
    });
}

app.UseHttpsRedirection();
app.MapGet("/cinemas", async (CinemaContext db) =>
{
    var generated = await db.Cinemas.Include(t => t.Actors).Include(t => t.Authors).Take(5).ToListAsync();
    return Results.Ok(generated);
})
.WithName("GetCinemas");
app.Run();

static async Task StartupDatabase(WebApplication app)
{
    var db = app.Services.CreateScope().ServiceProvider.GetService<CinemaContext>()!;
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
    db.Cinemas.AddRange(ReservationGenerator.GenerateCinemasFromThePast());
    await db.SaveChangesAsync();
}