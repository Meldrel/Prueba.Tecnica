using Prueba.Tecnica.Domain.Entities;

namespace Prueba.Tecnica.Domain.IRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Nos permite crear un nuevo usuario
        /// </summary>
        /// <param name="userName">Usuario</param>
        /// <param name="name">Name</param>
        /// <param name="password">Contraseña</param>
        /// <param name="role">Rol</param>
        /// <returns>User</returns>
        Task<User> CreateUser(string userName, string name, string password, string role);

        /// <summary>
        /// Nos permite obtener un usuario a través de su UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>User or nu</returns>
        Task<User?> GetUser(string userName);

        /// <summary>
        /// Obtiene un IQueryable de todos los usuarios
        /// </summary>
        /// <returns>IQueryable de users</returns>
        IQueryable<User> GetAllUsers();

        /// <summary>
        /// Intenta logear con un usuario
        /// </summary>
        /// <param name="userName">Usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>User si login es correcto o Null </returns>
        Task<User?> Login(string userName, string password);
    }
}
