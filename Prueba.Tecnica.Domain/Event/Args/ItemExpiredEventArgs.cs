namespace Prueba.Tecnica.Domain.Event.Args
{
    public class ItemExpiredEventArgs : EventArgs
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// Nombre
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        public DateTime? ExpiredTime { get; set; }

        public ItemExpiredEventArgs()
        { }

        public ItemExpiredEventArgs(Guid id, string name, DateTime expiredTime)
        {
            Id = id;
            Name = name;
            ExpiredTime = expiredTime;
        }
    }
}
