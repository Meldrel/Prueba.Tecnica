using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prueba.Tecnica.Aplication.AppService.Interfaces;
using Prueba.Tecnica.Aplication.Dto;
using Prueba.Tecnica.Aplication.Settings;
using Prueba.Tecnica.Domain.Entities;
using Prueba.Tecnica.Domain.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prueba.Tecnica.Aplication.AppService
{
    /// <summary>
    /// Implementa IItemAppService <see cref="ILoginAppService"/>
    /// </summary>
    public class LoginAppService : ILoginAppService
    {
        private readonly IUserRepository userRepository;
        private readonly JWTSettings jwtSettings;

        public LoginAppService(IUserRepository userRepository, IOptions<JWTSettings> jwtSettings)
        {
            this.userRepository = userRepository;
            this.jwtSettings = jwtSettings.Value;
        }

        public async Task<UserDto> GetUser(string userName)
        {
            var user = await userRepository.GetUser(userName);

            if (user == null)
                throw new ArgumentException($"No se ha encontrado un usuario con username {userName}");

            return new UserDto(user.UserName, user.Name, user.Role);
        }

        public async Task<UserLoginDto> Login(LoginDto login)
        {
            var user = await userRepository.Login(login.UserName, login.Password);

            if (user == null)
                throw new ArgumentException($"No se ha podido logear con {login.UserName}");


            string token = GenerateToken(user);

            return new UserLoginDto (user.UserName, user.Name, user.Role, token);
        }

        /// <summary>
        /// Genera un JWT token con la informaicón del usuario
        /// </summary>
        /// <param name="user">Usuario del que queremos generar el token</param>
        /// <returns>JWT token</returns>
        private string GenerateToken(User user)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var secret = jwtSettings.Secret;
            var key = Encoding.ASCII.GetBytes(secret);
            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var Token = TokenHandler.CreateToken(securityTokenDescriptor);
            return TokenHandler.WriteToken(Token);
        }
    }
}
