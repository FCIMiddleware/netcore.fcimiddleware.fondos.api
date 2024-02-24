using FluentValidation;

namespace netcore.fcimiddleware.fondos.application.Features.V1.CondIngEgrFondos.Commands.Create
{
    public class CreateCondIngEgrFondosCommandValidator : AbstractValidator<CreateCondIngEgrFondosCommand>
    {
        public CreateCondIngEgrFondosCommandValidator()
        {
            RuleFor(p => p.Descripcion)
                .NotNull().WithMessage("El campo Descripción no puede ser nula")
                .NotEmpty().WithMessage("El campo Descripción no puede estar en blanco")
                .MaximumLength(250).WithMessage("El campo Descripción no puede exceder los 250 caracteres");
            
            RuleFor(p => p.FondoId)
                .GreaterThan(0).WithMessage("El campo Fondo debe ser mayor a 0")
                .NotNull().WithMessage("El campo Fondo no puede ser nulo")
                .NotEmpty().WithMessage("El campo Fondo no puede estar en blanco");

            RuleFor(p => p.TpValorCptFondoId)
                .GreaterThan(0).WithMessage("El campo Clase debe ser mayor a 0")
                .NotNull().WithMessage("El campo Clase no puede ser nulo")
                .NotEmpty().WithMessage("El campo Clase no puede estar en blanco");
        }
    }
}
