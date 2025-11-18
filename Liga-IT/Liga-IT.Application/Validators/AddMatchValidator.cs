using FluentValidation;
using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Validators;

public class AddMatchValidator : AbstractValidator<AddMatchDto>
{
    public AddMatchValidator()
    {
        RuleFor(x => x.Round)
            .GreaterThan(0).WithMessage("La jornada debe ser mayor a 0");

        RuleFor(x => x.HomeClubId)
            .GreaterThan(0).WithMessage("El Id del club local debe ser mayor a 0");

        RuleFor(x => x.AwayClubId)
            .GreaterThan(0).WithMessage("El Id del club visitante debe ser mayor a 0")
            .NotEqual(x => x.HomeClubId).WithMessage("El club local y visitante no pueden ser el mismo");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado del partido no es válido");

        RuleFor(x => x.MatchDate)
            .NotEmpty().WithMessage("La fecha del partido es requerida");
    }
}
