namespace Prueba.Tecnica.Domain.Entities
{
    public static class Roles
    {
        public static string Usuario = "Usuario";
        public static string Administrador = "Administrador";

        /// <summary>
        /// Lista de roles admitidos por la aplicación. 
        /// Por evitar complejidades, se ha puesto en una lista, aunque deberían estar en BBDD
        /// </summary>
        public static readonly List<string> RolesList = new()
        { 
            Administrador,
            Usuario
        };
    }
}
