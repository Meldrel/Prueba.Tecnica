using Prueba.Tecnica.Aplication.Dto;

namespace Prueba.Tecnica.Aplication.AppService.Interfaces
{
    public interface IItemAppService
    {
        /// <summary>
        /// Nos permite crear un nuevo artículo
        /// </summary>
        /// <param name="itemCreateDto">Artículo para crear</param>
        /// <returns>Ver <see cref="ItemCreateDto"/></returns>
        public Task<ItemDto> CreateItem(ItemCreateDto itemCreateDto);

        /// <summary>
        /// Nos permite borrar un artículo por su name
        /// </summary>
        /// <param name="name">Name del artículo</param>
        /// <returns></returns>
        public Task DeleteItem(string name);

        /// <summary>
        /// Nos permite obtener un artículo por su name
        /// </summary>
        /// <param name="name">Name del artículo</param>
        /// <returns></returns>
        public Task<ItemDto> GetItem(string name);

        /// <summary>
        /// Obtiene una lista de los artículos que cumplan la query
        /// </summary>
        /// <returns>IQueryable de articulos</returns>
        public Task<List<ItemDto>> GetItems(ItemQueryDto itemQueryDto);
    }
}
