using Prueba.Tecnica.Domain.Entities;
using Prueba.Tecnica.Domain.Event.Args;

namespace Prueba.Tecnica.Domain.IRepository
{
    public interface IItemRepository
    {
        /// <summary>
        /// Evento que se dispara cuando se borra un item
        /// </summary>
        event EventHandler<ItemDeleteEventArgs> ItemRemoved;

        /// <summary>
        /// Nos permite crear un nuevo artículo
        /// </summary>
        /// <param name="name">Name del artículo</param>
        /// <param name="expirationDate">Fecha de vencimiento del artículo</param>
        /// <param name="type">Type del artículo</param>
        /// <returns>Item or ArgumentNullException</returns>
        Task<Item> CreateItem(string name, DateTime expirationDate, string type);

        /// <summary>
        /// Nos permite borrar un artículo por su name
        /// </summary>
        /// <param name="name">Name del artículo</param>
        Task DeleteItem(string name);

        /// <summary>
        /// Nos permite obtener un item por el name
        /// </summary>
        /// <param name="name">Name del artículo</param>
        /// <returns>Item or null.</returns>
        Task<Item?> GetItem(string name);

        /// <summary>
        /// Obtiene un IQueryable de todos los Items
        /// </summary>
        /// <returns>IQueryable de articulos</returns>
        IQueryable<Item> GetAllItems();
    }
}
