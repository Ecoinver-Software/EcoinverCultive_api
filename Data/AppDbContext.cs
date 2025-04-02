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
    public DbSet<Cultive> Cultives { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Gender> Gender { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Hacer que IdGenero en Gender sea único
        modelBuilder.Entity<Gender>()
            .HasIndex(g => g.IdGenero)
            .IsUnique();

        // Configurar la relación: CommercialNeeds.GeneroId -> Gender.IdGenero
        modelBuilder.Entity<CommercialNeeds>()
            .HasOne(cn => cn.Genero)
            .WithMany(g => g.CommercialNeeds)
            .HasForeignKey(cn => cn.GeneroId)
            .HasPrincipalKey(g => g.IdGenero)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
