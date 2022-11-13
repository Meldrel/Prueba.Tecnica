using System.Text.RegularExpressions;

namespace Prueba.Tecnica.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public DateTime CreationTime { get; private set; } = DateTime.UtcNow;
        public string UserName { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }

        /// <summary>
        /// Ctor para que use EF
        /// </summary>
        private User() { }

        /// <summary>
        /// Ctor para crear una clase Usuario válidad
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="name">Name</param>
        /// <param name="password">Contraseña</param>
        /// <param name="role">Rol</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Si UserName o Name están vacíos</exception>
        /// <exception cref="ArgumentOutOfRangeException">Si Role no se encuentra entre los roles permitidos</exception>
        public User(string userName, string name, string password, string role)
        {
            SetUser(userName, name, password, role);
        }

        private User SetUser(string userName, string name, string password, string role)
        {
            Id = Guid.NewGuid();
            UserName = !string.IsNullOrEmpty(userName) ? userName : throw new ArgumentNullException("UserName no puede estar vacío");
            Name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException("Name no puede estar vacío");
            Role = string.IsNullOrEmpty(role) 
                        ? throw new ArgumentNullException("Role no puede estar vacío")
                        :  Roles.RolesList.Contains(role) 
                                ? role 
                                : throw new ArgumentOutOfRangeException($"El rol {role} no está permitido");
            Password = password;

            return this;
        }

        /// <summary>
        /// Valida si la contraseña cumple los requisitos (una minúscula, una mayúscula un número y mas de 8 caracteres
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidatePassword(string password)
        {
            Regex validate = new Regex("^(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])[a-zA-Z0-9]{8,}");

            return validate.IsMatch(password);            
        }
    }
}
