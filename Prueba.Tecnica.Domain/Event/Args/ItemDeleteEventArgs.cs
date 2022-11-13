namespace Prueba.Tecnica.Domain.Event.Args
{
    public class ItemDeleteEventArgs : EventArgs
    {
        /// <summary>
        /// Id Elemento
        /// </summary>
        public Guid? Id { get; set; }
        /// <summary>
        /// Nombre 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Fecha en la que se produjo el borrado
        /// </summary>
        public DateTime? DeleteTime { get; set; }

        public ItemDeleteEventArgs()
        { }

        public ItemDeleteEventArgs(Guid id, string name, DateTime deleteTime)
        {
            Id = id;
            Name = name;
            DeleteTime = deleteTime;
        }
    }
}
