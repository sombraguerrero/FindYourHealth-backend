var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/__routes", (IEnumerable<EndpointDataSource> sources) =>
{
    return sources.SelectMany(s => s.Endpoints)
                  .OfType<RouteEndpoint>()
                  .Select(e => e.RoutePattern.RawText)
                  .ToList();
});


app.Run();
