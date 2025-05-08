using IntelligentCityApp.Accomodation.Hotels.API;
using IntelligentCityApp.Accomodation.Hotels.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.AddSqlServerDbContext<AccomodationContext>("HotelDb");

var app = builder.Build();

app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Hotel API");
    });
}

app.UseHttpsRedirection();
app.MapGet("/accomodations", async (AccomodationContext db, [FromQuery] DateTime searchDate) =>
{
    var rooms = await db.Accomodations
            .Include(a => a.Rooms)
            .ToListAsync();
    var result = rooms.Where(t => t.Rooms.Any(t => t.IsAvailable(searchDate)));
    var response = result.Select(t => new { t.Name, t.Address, RoomsCount = t.Rooms.Count }).ToList();
    return Results.Ok(response);
})
.WithName("GetAccomodations");

app.MapPost("/reset-db", async (AccomodationContext db) =>
{
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
    db.Accomodations.AddRange(ReservationGenerator.GenerateAccomodations());
    await db.SaveChangesAsync();
    return Results.Ok("Database reset");
});

app.Run();