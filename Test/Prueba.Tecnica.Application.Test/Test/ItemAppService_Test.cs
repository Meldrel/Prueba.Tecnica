namespace Prueba.Tecnica.Aplication.Test.Test
{
    public class ItemAppService_Test
    {
        private readonly IItemAppService itemAppSerice;

        public ItemAppService_Test()
        {
            var factory = new DbContextBuilder();
            itemAppSerice = new ItemAppService(new ItemRepository(factory.CreateContextForInMemory()));
        }

        [Fact]
        public async Task CreateItemOk()
        {
            var date = DateTime.UtcNow.AddDays(5);
            ItemDto item = await itemAppSerice.CreateItem(new ItemCreateDto("name1", date, "Type1"));

            item.Name.ShouldBe("name1");
            item.Type.ShouldBe("Type1");
            item.ExpirationDate.ShouldBe(date);
        }

        [Fact]
        public async Task GetItemOk()
        {
            ItemDto item = await itemAppSerice.GetItem("name1");

            item.Name.ShouldBe("name1");
            item.Type.ShouldBe("Type1");
        }

        [Fact]
        public void GetItemKo()
        {
            Should.Throw<ArgumentException>(async () => await itemAppSerice.GetItem("name123"))
                .Message.ShouldBe("No se ha podido encontrar un artículo con name name123");
        }

        [Fact]
        public async Task GetItemsOK1()
        {
            var items = await itemAppSerice.GetItems(new ItemQueryDto(type: "Type1"));

            items.Count.ShouldBe(2);
            items.ShouldAllBe(x => x.Type == "Type1");
            items.Any(x => x.Name == "name1").ShouldBeTrue();
            items.Any(x => x.Name == "name2").ShouldBeTrue();
        }

        [Fact]
        public async Task GetItemsOK3()
        {
            var items = await itemAppSerice.GetItems(new ItemQueryDto(minExpirationTime: DateTime.UtcNow.AddDays(12)));

            items.Count.ShouldBe(1);
            items.ShouldAllBe(x => x.Name == "name3");
        }
        [Fact]
        public async Task GetItemsOK4()
        {
            var items = await itemAppSerice.GetItems(new ItemQueryDto(maxExpirationTime: DateTime.UtcNow.AddDays(12)));

            items.Count.ShouldBe(2);
            items.ShouldAllBe(x => x.Type == "Type1");
            items.Any(x => x.Name == "name1").ShouldBeTrue();
            items.Any(x => x.Name == "name2").ShouldBeTrue();
        }
        [Fact]
        public async Task GetItemsOK2()
        {
            var items = await itemAppSerice.GetItems(new ItemQueryDto(minExpirationTime: DateTime.UtcNow.AddDays(8), maxExpirationTime: DateTime.UtcNow.AddDays(12)));

            items.Count.ShouldBe(1);
            items.ShouldAllBe(x => x.Name == "name2");
        }
    }
}
