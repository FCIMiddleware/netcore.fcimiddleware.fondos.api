using FluentValidation;

namespace netcore.fcimiddleware.fondos.application.Features.V1.Fondos.Commands.Create
{
    public class CreateFondosCommandValidator : AbstractValidator<CreateFondosCommand>
    {
        public CreateFondosCommandValidator()
        {
            RuleFor(p => p.Descripcion)
                .NotNull().WithMessage("El campo Descripción no puede ser nula")
                .NotEmpty().WithMessage("El campo Descripción no puede estar en blanco")
                .MaximumLength(250).WithMessage("El campo Descripción no puede exceder los 250 caracteres");

            RuleFor(p => p.MonedaId)
                .GreaterThan(0).WithMessage("El campo Moneda debe ser mayor a 0")
                .NotNull().WithMessage("El campo Moneda no puede ser nulo")
                .NotEmpty().WithMessage("El campo Moneda no puede estar en blanco");

            RuleFor(p => p.SocGerenteId)
                .GreaterThan(0).WithMessage("El campo Sociedad Gerente debe ser mayor a 0")
                .NotNull().WithMessage("El campo Sociedad Gerente no puede ser nulo")
                .NotEmpty().WithMessage("El campo Sociedad Gerente no puede estar en blanco");

            RuleFor(p => p.PaisId)
                .GreaterThan(0).WithMessage("El campo Pais debe ser mayor a 0")
                .NotNull().WithMessage("El campo Pais no puede ser nulo")
                .NotEmpty().WithMessage("El campo Pais no puede estar en blanco");

            RuleFor(p => p.SocDepositariaId)
                .GreaterThan(0).WithMessage("El campo Sociedad Depositaria debe ser mayor a 0")
                .NotNull().WithMessage("El campo Sociedad Depositaria no puede ser nulo")
                .NotEmpty().WithMessage("El campo Sociedad Depositaria no puede estar en blanco");
        }
    }
}
