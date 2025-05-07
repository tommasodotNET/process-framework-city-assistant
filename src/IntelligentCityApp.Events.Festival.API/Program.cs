using IntelligentCityApp.Events.Festival.API;
using IntelligentCityApp.Events.Festival.API.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.AddSqlServerDbContext<FestivalContext>("FestivalDb");

var app = builder.Build();
var db = app.Services.CreateScope().ServiceProvider.GetService<FestivalContext>()!;
await db.Database.EnsureDeletedAsync();
await db.Database.EnsureCreatedAsync();
db.Festivals.AddRange(ReservationGenerator.GenerateFestivals());
await db.SaveChangesAsync();

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
app.MapGet("/festivals", async (FestivalContext db) =>
{
    var generated = await db.Festivals.Take(5).ToListAsync();
    return Results.Ok(generated);
})
.WithName("GetFestivals");
app.Run();
