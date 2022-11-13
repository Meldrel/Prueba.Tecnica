using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Prueba.Tecnica.Infrastructure.EntityFramework
{
    /// <summary>
    /// Creamos DbContextFactory para crear y lanzar las migrations desde consola
    /// </summary>
    public class PruebaTecnicaDbContextFactory : IDesignTimeDbContextFactory<PruebaTecnicaDbContext>
    {
        public PruebaTecnicaDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder optionsBuilder = new();
            optionsBuilder.UseNpgsql("Host=localhost:5432;database=postgres-prueba;Username=postgres;Password=");

            return new PruebaTecnicaDbContext(optionsBuilder.Options);
        }
    }
}
