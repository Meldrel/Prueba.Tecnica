namespace Prueba.Tecnica.Aplication.Dto
{
    /// <summary>
    /// DTO para consultar una lista de artículos, según unos filtros concretos. 
    /// </summary>
    public class ItemQueryDto
    {
        /// <summary>
        /// Fecha minima de vencimiento por la que buscar. Tiene que ser UTC
        /// </summary>
        public DateTime? MinExpirationTime { get; set; } 
        /// <summary>
        /// Fecha máxima de vencimiento por la que buscar. Tiene que ser UTC
        /// </summary>
        public DateTime? MaxExpirationTime { get; set; }
        public string Type { get; set; }

        public ItemQueryDto(DateTime? minExpirationTime = null, DateTime? maxExpirationTime = null, string? type = null)
        {
            MinExpirationTime = minExpirationTime;
            MaxExpirationTime = maxExpirationTime;
            Type = type ?? string.Empty;
        }
    }
}
