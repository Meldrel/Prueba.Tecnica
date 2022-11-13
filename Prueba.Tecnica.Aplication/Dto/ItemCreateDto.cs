namespace Prueba.Tecnica.Aplication.Dto
{
    /// <summary>
    /// Clase para crear un artículo en la bbdd
    /// </summary>
    public class ItemCreateDto
    {
        public ItemCreateDto(string name, DateTime expirationDate, string type)
        {
            Name = name;
            ExpirationDate = expirationDate;
            Type = type;
        }

        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Type { get; set; }
    }
}
