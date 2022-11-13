namespace Prueba.Tecnica.Infrastructure.Test.Test
{
    public class ItemRepository_Test
    {
        private readonly PruebaTecnicaDbContext dbContext;
        private readonly IItemRepository itemRepository;

        public ItemRepository_Test()
        {
            dbContext = new DbContextBuilder().CreateContextForInMemory();
            itemRepository = new ItemRepository(dbContext);
        }

        [Fact]
        public async Task CreateItemOk()
        {
            var item = await itemRepository.CreateItem("ItemPrueba", DateTime.UtcNow.AddDays(15), "Type3");

            item.ShouldNotBeNull();

            var itemDb = dbContext.Items.First(x => x.Id == item.Id);

            item.ExpirationDate.ShouldBe(itemDb.ExpirationDate);
            item.CreationTime.ShouldBe(itemDb.CreationTime);
            item.Name.ShouldBe(itemDb.Name);
            item.Type.ShouldBe(itemDb.Type);
        }

        [Fact]
        public async Task DeleteItemOk()
        {
            itemRepository.ItemRemoved += (sender, args) =>
            {
                args.Name.ShouldBe("name2");
                args.Id.ShouldNotBeNull();
                args.DeleteTime.ShouldNotBeNull();
            };

            await itemRepository.DeleteItem("name2");

            var itemDb = dbContext.Items.FirstOrDefault(x => x.Name == "name2");

            itemDb.ShouldBeNull();
        }

        [Fact]
        public void DeleteItemKo()
        {
            Should.Throw<ArgumentOutOfRangeException>(async () => await itemRepository.DeleteItem("nameFail"))
                .ParamName.ShouldBe($"No existe un Item con name nameFail");
        }


        [Fact]
        public async Task GetItemOk()
        {
            var item = await itemRepository.GetItem("name2");

            item.ShouldNotBeNull();

            var itemDb = dbContext.Items.FirstOrDefault(x => x.Name == "name2");

            item.ExpirationDate.ShouldBe(itemDb.ExpirationDate);
            item.CreationTime.ShouldBe(itemDb.CreationTime);
            item.Name.ShouldBe(itemDb.Name);
            item.Type.ShouldBe(itemDb.Type);
        }

        [Fact]
        public async Task GetItemOk2()
        {
            var item = await itemRepository.GetItem("name213");

            item.ShouldBeNull();
        }

        [Fact]
        public async Task GetItemsOk()
        {
            var items = itemRepository.GetAllItems();

            items.ShouldNotBeNull();
            (await items.CountAsync()).ShouldBe(3);
        }

    }
}
