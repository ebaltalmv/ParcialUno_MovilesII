using Microsoft.EntityFrameworkCore;
using ODATAPokemonApi.Models;

namespace ODATAPokemonApi.Data;

public class PokemonDbContext(DbContextOptions<PokemonDbContext> options) : DbContext(options)
{
    public DbSet<PokemonCard> PokemonCards { get; set; }
    public DbSet<CardGrading> CardGradings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la entidad PokemonCard
        modelBuilder.Entity<PokemonCard>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(150);
            entity.Property(c => c.Category).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Rarity).HasMaxLength(50);
            entity.Property(c => c.Condition).HasMaxLength(50);
            entity.Property(c => c.EstimatedValue).HasPrecision(18, 2);

            // Relación 1 a N: Una carta tiene múltiples graduaciones
            entity.HasMany(c => c.Gradings)
                  .WithOne(g => g.PokemonCard)
                  .HasForeignKey(g => g.PokemonCardId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de la entidad CardGrading
        modelBuilder.Entity<CardGrading>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.GraderCompany).IsRequired().HasMaxLength(50);
            entity.Property(g => g.CertificateCode).IsRequired().HasMaxLength(50);
            entity.Property(g => g.Score).HasPrecision(4, 1);
        });
    }
}
