using System.Data;
using EcoinverGMAO_api.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User, Role, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
   

    // Aquí puedes agregar DbSet<T> para tus otras entidades, por ejemplo:
    // public DbSet<Producto> Productos { get; set; }
}
