using Prueba.Tecnica.Aplication.Dto;

namespace Prueba.Tecnica.Aplication.AppService.Interfaces
{
    public interface ILoginAppService
    {
        /// <summary>
        /// Método para intentar hacer un login. 
        /// </summary>
        /// <param name="loginDto">Dto con los datos para logearse</param>
        /// <returns>Ver <see cref="UserLoginDto"/></returns>
        /// <exception cref="ArgumentException">Si no se puede realizar el login</exception>
        Task<UserLoginDto> Login(LoginDto loginDto);

        /// <summary>
        /// Método para devolver un usuario a través de su nombre
        /// </summary>
        /// <param name="userName">Nombre del usuario que queremos consultar</param>
        /// <returns>Ver <see cref="UserDto"/></returns>
        /// <exception cref="ArgumentException">Si no existe el usuario</exception>
        Task<UserDto> GetUser(string userName);
    }
}
