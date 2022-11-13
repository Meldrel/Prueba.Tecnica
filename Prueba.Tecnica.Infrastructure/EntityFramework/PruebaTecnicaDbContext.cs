using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prueba.Tecnica.Domain.Entities;

namespace Prueba.Tecnica.Infrastructure.EntityFramework
{
    public class PruebaTecnicaDbContext : DbContext
    {
        /// <summary>
        /// Ctor publico que usamos cuando inyectamos el DbContext en la app
        /// </summary>
        /// <param name="options"></param>
        public PruebaTecnicaDbContext(DbContextOptions<PruebaTecnicaDbContext> options) : base(options)
        { }

        /// <summary>
        /// Ctor interno para usar en el DbContextFactory
        /// </summary>
        /// <param name="options"></param>
        internal PruebaTecnicaDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasKey(t => t.Id);

            modelBuilder.Entity<Item>().HasIndex(t => t.Name);

            modelBuilder.Entity<Item>(x =>
            {

                x.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

                x.Property(x => x.ExpirationDate)
                .IsRequired();

                x.Property(x => x.Type)
                .IsRequired();
            });

            modelBuilder.Entity<User>().HasKey(t => t.Id);

            modelBuilder.Entity<User>().HasIndex(t => t.UserName);

            modelBuilder.Entity<User>(x =>
            {

                x.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(50);

                x.Property(x => x.Password)
                .IsRequired();

                x.Property(x => x.Role)
                .IsRequired();
            });
        }
    }
}
