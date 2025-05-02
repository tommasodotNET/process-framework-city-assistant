var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/agents/accomodation", () =>
{
    return Results.Ok("I found two hotels in selected dates: Hotel A and Hotel B. Hotel A has 5 rooms available, while Hotel B has 10 rooms available.");
})
.WithName("GetAccomodation");

app.Run();