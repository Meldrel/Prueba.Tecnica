namespace Prueba.Tecnica.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; private set; } = DateTime.UtcNow;
        public string Name { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public string Type { get; private set; }

        /// <summary>
        /// Ctor para que use EF
        /// </summary>
        private Item() { }

        /// <summary>
        /// Ctor que nos permite crear una clase válida
        /// </summary>
        /// <param name="name">Name del Item</param>
        /// <param name="expirationDate">Fecha de vencimiento</param>
        /// <param name="tipo">Type del Item</param>
        /// <exception cref="ArgumentNullException">Si el name o el type son vacios o nulos</exception>
        /// <exception cref="ArgumentOutOfRangeException">Si la fecha de vencimiento no es mayor que la fecha actual</exception>
        public Item(string name, DateTime expirationDate, string type)
        {
            Id = Guid.NewGuid();
            Name = Type = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException("Name tiene no puede estar vacío");            
            ExpirationDate = expirationDate;
            Type = !string.IsNullOrEmpty(type) ? type : throw new ArgumentNullException("Type tiene no puede estar vacío");

            Validate();
        }


        /// <summary>
        /// Ctor que nos permite actualizar una clase válida
        /// </summary>
        /// <param name="expirationDate">Fecha de vencimiento</param>
        /// <param name="tipo">Type del Item</param>
        /// <exception cref="ArgumentNullException">Si el name o el type son vacios o nulos</exception>
        /// <exception cref="ArgumentOutOfRangeException">Si la fecha de vencimiento no es mayor que la fecha actual</exception>
        public void Update(DateTime expirationDate, string type)
        {
            ExpirationDate = expirationDate;
            Type = !string.IsNullOrEmpty(type) ? type : throw new ArgumentNullException("Type tiene no puede estar vacío");

            Validate();
        }

        /// <summary>
        /// Valida si la fecha de vencimiento es correcta
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Si la fecha de vencimiento no es mayor que la fecha actual</exception>
        private void Validate()
        {
            if (ExpirationDate < DateTime.UtcNow)
                throw new ArgumentOutOfRangeException($"La fecha de vencimiento {ExpirationDate} tiene que ser mayor que la acutal.");
        }
    }
}
