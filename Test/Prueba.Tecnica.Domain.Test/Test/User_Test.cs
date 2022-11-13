using Shouldly;

namespace Prueba.Tecnica.Domain.Test.Test
{
    public class User_Test
    {
        public User_Test() { }

        [Fact]
        public void CreateUserOk()
        {
            User user = new("userName", "name", "SDIdsai1wsq=", "Usuario");

            user.UserName.ShouldBe("userName");
            user.Name.ShouldBe("name");
            user.Password.ShouldBe("SDIdsai1wsq=");
            user.Role.ShouldBe("Usuario");
        }

        [Fact]
        public void CreateUserKo()
        {         
            Should.Throw<ArgumentNullException>(() => new User(string.Empty, "name", "SDIdsai1wsq=", "Usuario"))
                .ParamName.ShouldBe("UserName no puede estar vacío");
        }
        [Fact]
        public void CreateUserKo2()
        {         
            Should.Throw<ArgumentNullException>(() => new User("userName", string.Empty, "SDIdsai1wsq=", "Usuario"))
                .ParamName.ShouldBe("Name no puede estar vacío");
        }

        [Fact]
        public void CreateUserKo3()
        {         
            Should.Throw<ArgumentOutOfRangeException>(() => new User("userName", "name", "SDIdsai1wsq=", "FailRole"))
                .ParamName.ShouldBe("El rol FailRole no está permitido");
        }

        [Fact]
        public void CreateUserKo4()
        {         
            Should.Throw<ArgumentNullException>(() => new User("userName", "name", "SDIdsai1wsq=", string.Empty))
                .ParamName.ShouldBe("Role no puede estar vacío");
        }

        [Fact]
        public void ValidatePasswordOk()
        {
            User.ValidatePassword("123Qwert").ShouldBeTrue();
        }

        [Fact]
        public void ValidatePasswordKo1()
        {
            User.ValidatePassword("123qwert").ShouldBeFalse();
        }

        [Fact]
        public void ValidatePasswordKo2()
        {
            User.ValidatePassword("123QWERT").ShouldBeFalse();
        }

        [Fact]
        public void ValidatePasswordKo3()
        {
            User.ValidatePassword("qweQwert").ShouldBeFalse();
        }

        [Fact]
        public void ValidatePasswordKo4()
        {
            User.ValidatePassword("123qwer").ShouldBeFalse();
        }
    }
}
