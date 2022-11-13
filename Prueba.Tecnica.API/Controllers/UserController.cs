using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prueba.Tecnica.Aplication.AppService.Interfaces;
using Prueba.Tecnica.Aplication.Dto;
using Serilog;
using System.Security.Claims;

namespace Prueba.Tecnica.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "All")]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILoginAppService loginAppService;

        public UserController(ILoginAppService loginAppService)
        {
            this.loginAppService = loginAppService;
        }

        /// <summary>
        /// Devuelve información del usuario actual
        /// </summary>
        /// <returns>Si todo va correcto, devuelve un 200 con una lista de <see cref="UserDto"/>
        /// Si no va correcto, devuelve detalles del error</returns>
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var logger = Log.Logger.ForContext("Caller", "GetCurrentUser()");
            try
            {
                logger.Information("Init");

                var claimName = User.Claims.First(x => x.Type == ClaimTypes.Name);

                var user = await loginAppService.GetUser(claimName.Value);

                logger.Information("End");
                return Ok(user);
            }
            catch (Exception ex)
            {
                logger.Error("Error", ex);

                return Unauthorized();
            }
        }
    }
}
