using FluentValidation;

namespace Prueba.Tecnica.Aplication.Dto.Validators
{
    /// <summary>
    /// Validador para LoginDto
    /// Obligatorio los ods campos
    /// </summary>
    public class LoginDtoValidator : AbstractValidator<LoginDto>  
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
        }
    }
}
