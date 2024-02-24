using FluentValidation;

namespace netcore.fcimiddleware.fondos.application.Features.V1.SocGerentes.Commands.Create
{
    public class CreateSocGerentesCommandValidator : AbstractValidator<CreateSocGerentesCommand>
    {
        public CreateSocGerentesCommandValidator()
        {
            RuleFor(p => p.Descripcion)
                .NotNull().WithMessage("El campo Descripción no puede ser nula")
                .NotEmpty().WithMessage("El campo Descripción no puede estar en blanco")
                .MaximumLength(250).WithMessage("El campo Descripción no puede exceder los 250 caracteres");

        }
    }
}
