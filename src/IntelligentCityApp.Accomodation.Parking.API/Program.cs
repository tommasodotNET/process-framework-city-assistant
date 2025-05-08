using IntelligentCityApp.Accomodation.Parking.API;
using IntelligentCityApp.Accomodation.Parking.API.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.AddSqlServerDbContext<ParkingContext>("ParkingDB");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Hotel API");
    });
}

app.UseHttpsRedirection();
app.MapGet("/parkings", async (ParkingContext db) =>
{
    var generated = await db.Parkings.Include(a => a.UpcomingReservations).ToListAsync();
    return Results.Ok(generated);
})
.WithName("GetParkings");

app.MapPost("/reset-db", async (ParkingContext db) =>
{
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
    db.Parkings.AddRange(ReservationGenerator.GenerateParkings());
    await db.SaveChangesAsync();
    return Results.Ok("Database reset");
});

app.Run();
