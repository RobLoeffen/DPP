using DppApi.Domain.Interfaces;
using DppApi.Repository;
using DppApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSingleton<IDppRepository, JsonDppRepository>();
builder.Services.AddScoped<ICarbonCalculator, Iso14067Calculator>();
builder.Services.AddSingleton<IPefRepository, JsonPefRepository>();
builder.Services.AddScoped<IPefCalculator, PefCalculator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Enable CORS - Must be before UseAuthorization
app.UseCors("AllowVueFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
