using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prueba.Tecnica.Aplication.AppService.Interfaces;
using Prueba.Tecnica.Aplication.Dto;
using Serilog;

namespace Prueba.Tecnica.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginAppService loginAppService;
        private readonly IValidator<LoginDto> loginValidator;

        public LoginController(ILoginAppService loginAppService,
                               IValidator<LoginDto> loginValidator)
        {
            this.loginAppService = loginAppService;
            this.loginValidator = loginValidator;
        }

        /// <summary>
        /// Metodo para logearse en la app
        /// </summary>
        /// <param name="loginDto">Parametros para hacer el login</param>
        /// <returns>Si va correcto, devuelve un 200  y un <see cref="UserLoginDto"/>.
        /// Si hay algún problema devuelve los detalles del problema.
        /// Si no ha podido realizar el login, devuelve un 401</returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginDto>> Login(LoginDto loginDto)
        {
            var logger = Log.Logger.ForContext("Caller", "Login()")
                                   .ForContext("User", loginDto.UserName);
            try
            {
                logger.Information("Init login");

                var validation = loginValidator.Validate(loginDto);

                if (!validation.IsValid)
                    throw new ArgumentException($"Los campos no son válidos: {string.Join("", validation.Errors)}");

                var user = await loginAppService.Login(loginDto);

                logger.Information("End login");
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                logger.Error("Error in Login", ex);

                return Problem(detail: ex.StackTrace,
                              title: ex.Message);
            }
            catch (Exception ex)
            {
                logger.Error("Error in login", ex);

                return Unauthorized();
            }
        }
    }
}
