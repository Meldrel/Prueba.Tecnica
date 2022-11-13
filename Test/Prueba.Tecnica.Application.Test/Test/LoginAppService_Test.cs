using Microsoft.Extensions.Options;

namespace Prueba.Tecnica.Aplication.Test.Test
{
    public class LoginAppService_Test
    {
        private readonly ILoginAppService loginAppService;

        public LoginAppService_Test()
        {
            var factory = new DbContextBuilder();
            var settings = Options.Create(new JWTSettings { Secret = ">ioJH()H(i9H89hD)(IhDHY¿=" });
            loginAppService = new LoginAppService(new UserRepository(factory.CreateContextForInMemory()), settings);
        }

        [Fact]
        public async Task GetUserOk()
        {
            UserDto user = await loginAppService.GetUser("Admin");

            user.UserName.ShouldBe("Admin");
            user.Name.ShouldBe("Admin");
            user.Role.ShouldBe("Administrador");
        }

        [Fact]
        public void GetUserKo()
        {
            Should.Throw<ArgumentException>(async () => await loginAppService.GetUser("UserFail"))
                .Message.ShouldBe($"No se ha encontrado un usuario con username UserFail");
        }

        [Fact]
        public async Task LoginOk()
        {
            UserLoginDto userLogin = await loginAppService.Login(new LoginDto("Admin", "123Qwert"));

            userLogin.Token.ShouldNotBeNull();
            userLogin.UserName.ShouldBe("Admin");
            userLogin.Name.ShouldBe("Admin");
            userLogin.Role.ShouldBe("Administrador");
        }

        [Fact]
        public void LoginKo()
        {
            Should.Throw<ArgumentException>(async () => await loginAppService.Login(new LoginDto("UserFail", "123Qwert")))
                .Message.ShouldBe($"No se ha podido logear con UserFail");
        }
    }
}
