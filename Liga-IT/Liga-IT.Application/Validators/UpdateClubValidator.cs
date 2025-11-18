using FluentValidation;
using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Validators;

public class UpdateClubValidator : AbstractValidator<UpdateClubDto>
{
    public UpdateClubValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id del club debe ser mayor a 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del club es requerido")
            .MaximumLength(100).WithMessage("El nombre del club no puede exceder los 100 caracteres");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("La ciudad del club es requerida")
            .MaximumLength(50).WithMessage("La ciudad del club no puede exceder los 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email del club es requerido")
            .EmailAddress().WithMessage("El email del club no es válido")
            .MaximumLength(100).WithMessage("El email del club no puede exceder los 100 caracteres");

        RuleFor(x => x.NumberOfPartners)
            .GreaterThanOrEqualTo(0).WithMessage("El número de socios no puede ser negativo");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("El teléfono del club es requerido")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("El teléfono del club no es válido");

        RuleFor(x => x.StadiumName)
            .NotEmpty().WithMessage("El nombre del estadio es requerido")
            .MaximumLength(100).WithMessage("El nombre del estadio no puede exceder los 100 caracteres");
    }
}
