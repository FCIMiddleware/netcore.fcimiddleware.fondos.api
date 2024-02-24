using FluentValidation;

namespace netcore.fcimiddleware.fondos.application.Features.V1.TpValorCptFondos.Commands.Create
{
    public class CreateTpValorCptFondosCommandValidator : AbstractValidator<CreateTpValorCptFondosCommand>
    {
        public CreateTpValorCptFondosCommandValidator()
        {
            RuleFor(p => p.Descripcion)
                .NotNull().WithMessage("El campo Descripción no puede ser nula")
                .NotEmpty().WithMessage("El campo Descripción no puede estar en blanco")
                .MaximumLength(250).WithMessage("El campo Descripción no puede exceder los 250 caracteres");

        }
    }
}
