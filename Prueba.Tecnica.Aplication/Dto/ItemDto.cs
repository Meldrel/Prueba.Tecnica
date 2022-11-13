namespace Prueba.Tecnica.Aplication.Dto
{
    /// <summary>
    /// DTO para devolver la información de un artículo a través de nuestros endpoint
    /// </summary>
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Type { get; set; }
    }
}
