namespace Prueba.Tecnica.Aplication.Dto
{
    /// <summary>
    /// DTO para devolver la información de un usuario a través de nuestros endpoint
    /// </summary>
    public class UserDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public UserDto(string userName, string name, string role)
        {
            UserName = userName;
            Name = name;
            Role = role;    
        }
    }
}
