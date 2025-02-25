using Microsoft.EntityFrameworkCore;

namespace EcoinverGMAO_api.data
{
    public class AppDbContext : DbContext
    {
        // Constructor opcional para inyección de dependencia
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Ejemplo de DbSet - Tus entidades irían aquí
        public DbSet<Producto> Productos { get; set; }
    }
}
