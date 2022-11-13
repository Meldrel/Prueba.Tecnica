using Microsoft.EntityFrameworkCore;
using Prueba.Tecnica.Domain.Entities;
using Prueba.Tecnica.Domain.Event.Args;
using Prueba.Tecnica.Domain.IRepository;
using Prueba.Tecnica.Infrastructure.EntityFramework;

namespace Prueba.Tecnica.Infrastructure.Repository
{
    /// <summary>
    /// Implementación de la interfaz IItemRepository
    /// <see cref="IItemRepository"/>
    /// </summary>
    public class ItemRepository : IItemRepository
    {
        public event EventHandler<ItemDeleteEventArgs>? ItemRemoved;

        private readonly PruebaTecnicaDbContext context;

        public ItemRepository(PruebaTecnicaDbContext context)
        {
            this.context = context;
        }

        public async Task<Item> CreateItem(string name, DateTime expirationDate, string type)
        {
            var item = new Item(name, expirationDate, type);

            await context.Items.AddAsync(item);
            await context.SaveChangesAsync();

            return item;
        }

        public async Task DeleteItem(string name)
        {
            var item = await GetItem(name);

            if (item == null)
                throw new ArgumentOutOfRangeException($"No existe un Item con name {name}");

            context.Items.Remove(item);

            await context.SaveChangesAsync();

            ItemRemoved?.Invoke(this, new ItemDeleteEventArgs(item.Id, item.Name, DateTime.UtcNow));
        }

        public IQueryable<Item> GetAllItems()
        {
            return context.Items.AsQueryable();
        }

        public Task<Item?> GetItem(string name)
        {
            var item = context.Items.FirstOrDefaultAsync(x => x.Name == name);

            return item;
        }
    }
}
