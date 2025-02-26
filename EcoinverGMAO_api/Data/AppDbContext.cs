using System.Data;
using EcoinverGMAO_api.Models;
using EcoinverGMAO_api.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User, Role, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
   

    public DbSet<JobPosition> WorkerType { get; set; }
}
