using IntelligentCityApp.Accomodation.Rental.API;
using IntelligentCityApp.Accomodation.Rental.API.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.AddSqlServerDbContext<RentalContext>("RentalDB");

var app = builder.Build();
var db = app.Services.CreateScope().ServiceProvider.GetService<RentalContext>()!;
//await db.Database.EnsureDeletedAsync();
//await db.Database.EnsureCreatedAsync();
db.Rentals.AddRange(ReservationGenerator.GenerateRentals());
await db.SaveChangesAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Hotel API");
    });
}

app.UseHttpsRedirection();
app.MapGet("/rental", async (RentalContext db) =>
{
    var generated = await db.Rentals.Include(a => a.Vehicles).ToListAsync();
    return Results.Ok(generated);
})
.WithName("GetRentals");
app.Run();
