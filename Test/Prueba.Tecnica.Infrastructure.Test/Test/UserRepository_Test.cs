namespace Prueba.Tecnica.Infrastructure.Test.Test
{
    public class UserRepository_Test
    {
        private readonly PruebaTecnicaDbContext dbContext;
        private readonly IUserRepository userRepository;

        public UserRepository_Test()
        {
            dbContext = new DbContextBuilder().CreateContextForInMemory();
            userRepository = new UserRepository(dbContext);
        }

        [Fact]
        public async Task CreateUserOk()
        {
            var user = await userRepository.CreateUser("Admin2", "Admin", "123Qwert", "Administrador");

            var userInDb = dbContext.Users.First(x => x.Id == user.Id);

            user.Id.ShouldBe(userInDb.Id);
            user.Name.ShouldBe(userInDb.Name);
            user.UserName.ShouldBe(userInDb.UserName);
            user.CreationTime.ShouldBe(userInDb.CreationTime);
            user.Role.ShouldBe(userInDb.Role);
        }

        [Fact]
        public void CreateUserKo()
        {
            Should.Throw<ArgumentException>(async () => await userRepository.CreateUser("Admin2", "Admin", "12Qwer", "Administrador"))
                .Message.ShouldBe("La contraseña tiene que tener mas de 8 carácteres y usar mayúsculas, minúsculas y números");            
        }


        [Fact]
        public async Task GetUserOk()
        {
            var user = await userRepository.GetUser("Admin");

            var userInDb = dbContext.Users.First(x => x.Id == user.Id);

            user.ShouldNotBeNull();
            user.Id.ShouldBe(userInDb.Id);
            user.Name.ShouldBe(userInDb.Name);
            user.UserName.ShouldBe(userInDb.UserName);
            user.CreationTime.ShouldBe(userInDb.CreationTime);
            user.Role.ShouldBe(userInDb.Role);
        }

        [Fact]
        public async Task GetUserOK2()
        {
            var user = await userRepository.GetUser("Otro");

            user.ShouldBeNull();
        }


        [Fact]
        public async Task GetAllUsersOk()
        {
            var usersDb = userRepository.GetAllUsers();
            
            usersDb.ShouldNotBeNull();
            (await usersDb.CountAsync()).ShouldBe(2);
        }

        [Fact]
        public async Task LoginOk()
        {
            var userLogin = await userRepository.Login("Admin", "123Qwert");

            userLogin.ShouldNotBeNull();

            var userInDb = dbContext.Users.First(x => x.Id == userLogin.Id);

            userLogin.Id.ShouldBe(userInDb.Id);
            userLogin.Name.ShouldBe(userInDb.Name);
            userLogin.UserName.ShouldBe(userInDb.UserName);
            userLogin.CreationTime.ShouldBe(userInDb.CreationTime);
            userLogin.Role.ShouldBe(userInDb.Role);
        }

        [Fact]
        public async Task LoginKo()
        { 
            var userLogin = await userRepository.Login("UserFaiul", "123Qwert");

            userLogin.ShouldBeNull();
        }

    }
}
