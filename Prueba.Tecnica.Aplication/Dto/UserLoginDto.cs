namespace Prueba.Tecnica.Aplication.Dto
{
    /// <summary>
    /// DTO que extiende UserDto, para añadirle el token después de un login exitoso
    /// <see cref="UserDto"/>
    /// </summary>
    public class UserLoginDto : UserDto
    {
        public string Token { get; set; }

        public UserLoginDto(string userName, string name, string role, string token) : base(userName, name, role)
        {
            Token = token;
        }
    }
}
