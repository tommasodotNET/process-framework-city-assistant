using IntelligentCityApp.Accomodation.Hotels.API;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddOpenApi();

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
app.MapGet("/accomodations", () =>
{
    var generated = ReservationGenerator.GenerateAccomodations();
    return Results.Ok(generated);
})
.WithName("GetAccomodations");
app.Run();
