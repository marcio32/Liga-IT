using FluentValidation;
using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Validators;

public class UpdateMatchValidator : AbstractValidator<UpdateMatchDto>
{
    public UpdateMatchValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id del partido debe ser mayor a 0");

        RuleFor(x => x.Round)
            .GreaterThan(0).WithMessage("La jornada debe ser mayor a 0");

        RuleFor(x => x.HomeClubId)
            .GreaterThan(0).WithMessage("El Id del club local debe ser mayor a 0");

        RuleFor(x => x.AwayClubId)
            .GreaterThan(0).WithMessage("El Id del club visitante debe ser mayor a 0")
            .NotEqual(x => x.HomeClubId).WithMessage("El club local y visitante no pueden ser el mismo");

        RuleFor(x => x.HomeScore)
            .GreaterThanOrEqualTo(0).When(x => x.HomeScore.HasValue).WithMessage("El marcador local no puede ser negativo");

        RuleFor(x => x.AwayScore)
            .GreaterThanOrEqualTo(0).When(x => x.AwayScore.HasValue).WithMessage("El marcador visitante no puede ser negativo");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado del partido no es válido");

        RuleFor(x => x.MatchDate)
            .NotEmpty().WithMessage("La fecha del partido es requerida");
    }
}
