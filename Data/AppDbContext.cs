using System.Data;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Entities;
using EcoinverGMAO_api.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User, Role, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Center> Centers { get; set; }
    public DbSet<CommercialNeeds> CommercialNeeds { get; set; }
    public DbSet<CommercialNeedsPlanning> CommercialNeedsPlanning { get; set; }
    public DbSet<CommercialNeedsPlanningDetails> CommercialNeedsPlanningDetails { get; set; }

    public DbSet<Cultive> Cultives { get; set; }
    public DbSet<CultivePlanning> CultivesPlanning { get; set; }
    public DbSet<CultivePlanningDetails> CultivesPlanningDetails { get; set; }
    public DbSet<CultiveProduction> CultivesProduction { get; set; }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Gender> Gender { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Llamar al base para no romper la configuración 
        // que IdentityDbContext hace internamente
        base.OnModelCreating(modelBuilder);

        // 1) Configura Cultive → CultiveProduction
        modelBuilder.Entity<CultiveProduction>()
            .HasOne(p => p.Cultive)                 // cada producción apunta a un Cultive
            .WithMany(c => c.Productions)           // un Cultive puede tener muchas productions
            .HasForeignKey(p => p.CultiveId)        // la columna CultiveId es la FK
            .OnDelete(DeleteBehavior.Cascade);      // al borrar un Cultive, borramos sus productions

        // 2. CONFIGURACIÓN 1:1 ENTRE CultivePlanning ↔ Gender
        modelBuilder.Entity<CultivePlanning>()
            .HasOne(p => p.Genero)                   // cada Planning tiene un Gender
            .WithOne(g => g.CultivePlanning)        // cada Gender apunta a un único Planning
            .HasForeignKey<CultivePlanning>(p => p.IdGenero)
            .IsRequired(false)                      //hace que sea nullable
            .OnDelete(DeleteBehavior.Restrict);     // evita cascada si borras el Gender


    }
}
