using Shouldly;

namespace Prueba.Tecnica.Aplication.Test.Test
{
    public class Item_Test
    {
        public Item_Test() { }

        [Fact]
        public void CreateItemOk()
        {
            var date = DateTime.UtcNow.AddDays(5);
            Item item = new("name", date, "Type1");

            item.Name.ShouldBe("name");
            item.ExpirationDate.ShouldBe(date);
            item.Type.ShouldBe("Type1");
        }

        [Fact]
        public void CreateItemKo1()
        {
            DateTime date = DateTime.UtcNow.AddDays(-5);
            Should.Throw<ArgumentOutOfRangeException>(() => new Item("name", date, "Type1"))
                .ParamName.ShouldBe($"La fecha de vencimiento {date} tiene que ser mayor que la acutal.");

        }

        [Fact]
        public void CreateItemKo2()
        {
            Should.Throw<ArgumentNullException>(() => new Item(null, DateTime.UtcNow.AddDays(5), "Type1"))
                .ParamName.ShouldBe("Name tiene no puede estar vacío");

        }

        [Fact]
        public void CreateItemKo3()
        {
            Should.Throw<ArgumentNullException>(() => new Item("name", DateTime.UtcNow.AddDays(5), null))
                .ParamName.ShouldBe("Type tiene no puede estar vacío");

        }

        [Fact]
        public void UpdateItemOk()
        {
            var date = DateTime.UtcNow.AddDays(5);
            Item item = new("name", date, "Type1");

            var dateUpdated = DateTime.UtcNow.AddDays(10);
            item.Update(dateUpdated, "Type2");

            item.Name.ShouldBe("name");
            item.ExpirationDate.ShouldBe(dateUpdated);
            item.Type.ShouldBe("Type2");
        }

        [Fact]
        public void UpdateItemKo1()
        {
            var date = DateTime.UtcNow.AddDays(5);
            Item item = new("name", date, "Type1");

            DateTime dateToUpdate = DateTime.UtcNow.AddDays(-5);
            Should.Throw<ArgumentOutOfRangeException>(() => item.Update(dateToUpdate, "Type1"))
                .ParamName.ShouldBe($"La fecha de vencimiento {dateToUpdate} tiene que ser mayor que la acutal.");
        }

        [Fact]
        public void UpdateItemKo2()
        {
            var date = DateTime.UtcNow.AddDays(5);
            Item item = new("name", date, "Type1");
            var dateUpdated = DateTime.UtcNow.AddDays(10);

            Should.Throw<ArgumentNullException>(() => item.Update(dateUpdated, string.Empty))
                .ParamName.ShouldBe("Type tiene no puede estar vacío");
        }


    }
}
