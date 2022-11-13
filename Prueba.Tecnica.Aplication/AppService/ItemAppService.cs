using Prueba.Tecnica.Aplication.Dto;
using Prueba.Tecnica.Aplication.AppService.Interfaces;
using Prueba.Tecnica.Domain.IRepository;
using Prueba.Tecnica.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Prueba.Tecnica.Aplication.Extensions;

namespace Prueba.Tecnica.Aplication.AppService
{
    /// <summary>
    /// Implementa IItemAppService <see cref="IItemAppService"/>
    /// </summary>
    public class ItemAppService : IItemAppService
    {
        private readonly IItemRepository itemRepository;

        public ItemAppService(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task DeleteItem(string name)
        {
            await itemRepository.DeleteItem(name);
        }

        public async Task<ItemDto> CreateItem(ItemCreateDto itemCreateDto)
        {
            Item item = await itemRepository.CreateItem(itemCreateDto.Name, itemCreateDto.ExpirationDate, itemCreateDto.Type);

            return ConvertItem(item);
        }

        public async Task<ItemDto> GetItem(string name)
        {
            Item item = await itemRepository.GetItem(name);

            if (item == null)
                throw new ArgumentException($"No se ha podido encontrar un artículo con name {name}");

            return ConvertItem(item);
        }

        public async Task<List<ItemDto>> GetItems(ItemQueryDto itemQueryDto)
        {
            var items = await itemRepository.GetAllItems()
                              .WhereIf(!string.IsNullOrEmpty(itemQueryDto.Type), x => x.Type == itemQueryDto.Type)
                              .WhereIf(itemQueryDto.MaxExpirationTime != null, x => x.ExpirationDate <= itemQueryDto.MaxExpirationTime)
                              .WhereIf(itemQueryDto.MinExpirationTime != null, x => x.ExpirationDate >= itemQueryDto.MinExpirationTime)
                              .ToListAsync();

            return items.ConvertAll(x => ConvertItem(x));
        }

        /// <summary>
        /// Convierte un <see cref="Item"/> a un <see cref="ItemDto"/>
        /// </summary>
        /// <param name="item">Item que queremos convertir</param>
        /// <returns>ItemDto con los campos del item</returns>
        private ItemDto ConvertItem(Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                ExpirationDate = item.ExpirationDate,
                Name = item.Name,
                Type = item.Type
            };
        }
    }
}