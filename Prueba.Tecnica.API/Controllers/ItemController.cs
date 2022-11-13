using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prueba.Tecnica.Aplication.AppService.Interfaces;
using Prueba.Tecnica.Aplication.Dto;
using Serilog;

namespace Prueba.Tecnica.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "All")]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemAppService itemAppService;
        private readonly IValidator<ItemCreateDto> itemCrearteValidator;

        public ItemController(IItemAppService itemAppService,
                              IValidator<ItemCreateDto> itemCrearteValidator)
        {
            this.itemAppService = itemAppService;
            this.itemCrearteValidator = itemCrearteValidator;
        }

        /// <summary>
        /// Obtiene un item a través de su naem
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Si todo va correcto, devuelve un 200 con <see cref="ItemDto"/>
        /// Si no va correcto, devuelve detalles del error</returns>
        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult<ItemDto>> GetItem(string name)
        {
            var logger = Log.Logger.ForContext("Caller", "GetItem()");
            try
            {
                logger.Information("Init ");
                ItemDto item = await itemAppService.GetItem(name);

                logger.Information("End");
                return Ok(item);
            }
            catch (Exception ex)
            {
                logger.Error("Error in GetItem", ex);

                return Problem(detail: ex.StackTrace,
                              title: ex.Message);
            }
        }

        /// <summary>
        /// Crea un item 
        /// </summary>
        /// <param name="itemCreateDto">Datos del item a crear</param>
        /// <returns>Si todo va correcto, devuelve un 200 con <see cref="ItemDto"/>
        /// Si no va correcto, devuelve detalles del error</returns>
        /// <exception cref="ArgumentException">Si no cumple los criterios de validación <see cref="ItemCreateDtoValidator"/></exception>
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItem(ItemCreateDto itemCreateDto)
        {
            var logger = Log.Logger.ForContext("Caller", "CreateItem()");
            try
            {
                logger.Information("Init");
                var validation = itemCrearteValidator.Validate(itemCreateDto);

                if (!validation.IsValid)
                    throw new ArgumentException($"Los campos no son válidos: {string.Join("", validation.Errors)}");

                ItemDto item = await itemAppService.CreateItem(itemCreateDto);

                logger.Information("End");
                return Ok(item);
            }
            catch (Exception ex)
            {
                logger.Error("Error in CreateItem", ex);

                return Problem(detail: ex.StackTrace,
                              title: ex.Message);
            }
        }

        /// <summary>
        /// Método para listar los items a partir de unos filtros
        /// </summary>
        /// <param name="type">Filtro de tipo</param>
        /// <param name="maxExpirationTime">Filtro de fecha de expiración max</param>
        /// <param name="minExpirationTime">Filtro de fecha de expiración min</param>
        /// <returns>Si todo va correcto, devuelve un 200 con una lista de <see cref="ItemDto"/>
        /// Si no va correcto, devuelve detalles del error</returns>
        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<List<ItemDto>>> GetItems([FromQuery] string? type, [FromQuery] DateTime? maxExpirationTime, [FromQuery] DateTime? minExpirationTime)
        {
            var logger = Log.Logger.ForContext("Caller", "GetItems()");
            try
            {
                logger.Information("Init");
                List<ItemDto> items = await itemAppService.GetItems(new ItemQueryDto
                {
                    Type = type,
                    MaxExpirationTime = maxExpirationTime,
                    MinExpirationTime = minExpirationTime
                });

                logger.Information("End");
                return Ok(items);
            }
            catch (Exception ex)
            {
                logger.Error("Error in GetItems", ex);

                return Problem(detail: ex.StackTrace,
                              title: ex.Message);
            }
        }

        /// <summary>
        /// Borra un item a partir de su nombre
        /// </summary>
        /// <param name="name">Nombre del item que queremos borrar</param>
        /// <returns>Si todo va correcto, devuelve un 200
        /// Si no va correcto, devuelve detalles del error</returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpDelete]
        [Route("{name}")]
        public async Task<ActionResult> DeleteItem(string name)
        {
            var logger = Log.Logger.ForContext("Caller", "DeleteItem()");
            try
            {
                logger.Information("Init");
                await itemAppService.DeleteItem(name);

                logger.Information("End");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error("Error in DeleteItem", ex);

                return Problem(detail: ex.StackTrace,
                              title: ex.Message);
            }
        }

    }
}
