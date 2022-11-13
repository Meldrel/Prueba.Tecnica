using FluentValidation;

namespace Prueba.Tecnica.Aplication.Dto.Validators
{
    /// <summary>
    /// Validador para ItemCreateDto
    /// Obligatorio los tres campos, y que ExirationDate sea mayor que la fecha actual. 
    /// </summary>
    public class ItemCreateDtoValidator : AbstractValidator<ItemCreateDto>  
    {
        public ItemCreateDtoValidator()
        {
            RuleFor(x => x.ExpirationDate).GreaterThan(DateTime.UtcNow);
            RuleFor(x => x.Type).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
