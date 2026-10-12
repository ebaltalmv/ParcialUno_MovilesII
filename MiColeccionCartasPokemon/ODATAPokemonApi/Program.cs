using System.Diagnostics;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using ODATAPokemonApi.Data;
using ODATAPokemonApi.Models;

var builder = WebApplication.CreateSlimBuilder(args);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configuración del modelo OData EDM
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<PokemonCard>("PokemonCards");
modelBuilder.EntitySet<CardGrading>("CardGradings");

// Controladores con OData
builder.Services.AddControllers().AddOData(options =>
{
    options.AddRouteComponents("odata", modelBuilder.GetEdmModel());
    options.EnableQueryFeatures(null);
});

// Base de Datos SQLite
builder.Services.AddDbContext<PokemonDbContext>(options =>
    options.UseSqlite("Data Source=pokemon_cards.db"));

var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();

// Inicialización de la base de datos y carga de datos iniciales (Seed)
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var db = serviceScope.ServiceProvider.GetRequiredService<PokemonDbContext>();
    db.Database.EnsureCreated();
    Seeder.SeedData(db);
}

if (app.Environment.IsDevelopment())
{
    // Abre automáticamente el navegador en la ruta OData al iniciar
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        try
        {
            var serverUrl = app.Urls.FirstOrDefault() ?? "http://localhost:5123";
            Process.Start(new ProcessStartInfo
            {
                FileName = $"{serverUrl}/odata/PokemonCards",
                UseShellExecute = true
            });
        }
        catch
        {
            // En caso de entorno sin interfaz gráfica, se ignora
        }
    });
}

app.Run();
