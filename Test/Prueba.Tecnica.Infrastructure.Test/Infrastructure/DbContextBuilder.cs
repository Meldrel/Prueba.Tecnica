using System.Diagnostics.CodeAnalysis;

namespace Prueba.Tecnica.Infrastructure.Test.Infrastructure
{
    public class DbContextBuilder 
    {
        /// <summary>
        /// Builder que construye un DbContext para usar en los test
        /// </summary>
        /// <returns></returns>
        public PruebaTecnicaDbContext CreateContextForInMemory() 
        {
            var option = new DbContextOptionsBuilder<PruebaTecnicaDbContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var dbContext = new PruebaTecnicaDbContext(option);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            AddDataToDb(dbContext);

            return dbContext;
        }

        private void AddDataToDb(PruebaTecnicaDbContext dbContext)
        {
            Item item1 = new("name1", DateTime.UtcNow.AddDays(5), "Type1");
            Item item2 = new("name2", DateTime.UtcNow.AddDays(10), "Type1");
            Item item3 = new("name3", DateTime.UtcNow.AddDays(15), "Type2");

            User user1 = new("Admin", "Admin", "MTIzUXdlcnQ=", "Administrador");
            User user2 = new("User", "User", "MTIzUXdlcnQ=", "Usuario");

            dbContext.Items.AddRange(new Item[] { item1, item2, item3 });
            dbContext.Users.AddRange(new User[] { user1, user2 });
            dbContext.SaveChanges();
        }
    }
}
