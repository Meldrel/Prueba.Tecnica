using Microsoft.EntityFrameworkCore;
using Prueba.Tecnica.Domain.Entities;
using Prueba.Tecnica.Domain.IRepository;
using Prueba.Tecnica.Infrastructure.EntityFramework;
using System.Text;

namespace Prueba.Tecnica.Infrastructure.Repository
{    /// <summary>
     /// Implementación de la interfaz IUserRepository
     /// <see cref="IUserRepository"/>
     /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly PruebaTecnicaDbContext context;

        public UserRepository(PruebaTecnicaDbContext context)
        {
            this.context = context;
        }

        public async Task<User> CreateUser(string userName, string name, string password, string role)
        {
            if(!User.ValidatePassword(password))
                throw new ArgumentException("La contraseña tiene que tener mas de 8 carácteres y usar mayúsculas, minúsculas y números");

            string passwordEncoded = EncodePassword(password);

            var user = new User(userName, name, passwordEncoded, role);

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }
        
        public IQueryable<User> GetAllUsers()
        {
            return context.Users.AsQueryable(); 
        }

        public async Task<User?> GetUser(string userName)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            return user;
        }

        public async Task<User?> Login(string userName, string password)
        {
            var user = await GetUser(userName);

            if (user == null || EncodePassword(password) != user.Password)
                return null;

            return user;
        }

        private string EncodePassword(string password)
        {
            //TODO Implementar sistema de codificación real
            var encodedPasword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            return encodedPasword;        
        }
    }
}
