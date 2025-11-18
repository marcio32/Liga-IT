using FluentValidation;
using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Validators;

public class AddPlayerValidator : AbstractValidator<AddPlayerDto>
{
    public AddPlayerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(50).WithMessage("El apellido no puede exceder los 50 caracteres");

        RuleFor(x => x.Age)
            .GreaterThan(0).WithMessage("La edad debe ser mayor a 0")
            .LessThan(100).WithMessage("La edad debe ser menor a 100");

        RuleFor(x => x.JerseyNumber)
            .GreaterThan(0).WithMessage("El número de camiseta debe ser mayor a 0")
            .LessThanOrEqualTo(99).WithMessage("El número de camiseta debe ser menor o igual a 99");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("La posición es requerida");

        RuleFor(x => x.Nationality)
            .NotEmpty().WithMessage("La nacionalidad es requerida");

        RuleFor(x => x.Height)
            .GreaterThan(0).WithMessage("La altura debe ser mayor a 0");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("El peso debe ser mayor a 0");

        RuleFor(x => x.ClubId)
            .GreaterThan(0).WithMessage("El Id del club debe ser mayor a 0");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow).WithMessage("La fecha de nacimiento debe ser anterior a hoy");
    }
}
