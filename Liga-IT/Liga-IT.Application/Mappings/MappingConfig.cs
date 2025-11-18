using Liga_IT.Application.DTOs;
using Liga_IT.Domain.Entities;
using Mapster;
using FluentValidation.Results;

namespace Liga_IT.Application.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        //Club Mappings
        config.NewConfig<Club, ClubDto>();
        config.NewConfig<AddClubDto, Club>()
            .Map(dest => dest.CreateAt, src => DateTime.UtcNow);

        //Referee Mappings
        config.NewConfig<Referee, RefereeDto>();
        config.NewConfig<AddRefereeDto, Referee>()
            .Map(dest => dest.IsActive, src => true)
            .Map(dest => dest.CreateAt, src => DateTime.UtcNow);

        //Match Mappings
        config.NewConfig<Match, MatchDto>();
        config.NewConfig<AddMatchDto, Match>()
            .Map(dest => dest.CreateAt, src => DateTime.UtcNow);

        //Player Mappings
        config.NewConfig<Player, PlayerDto>();
        config.NewConfig<AddPlayerDto, Player>()
            .Map(dest => dest.IsActive, src => true)
            .Map(dest => dest.CreateAt, src => DateTime.UtcNow);

        //Validation Mappings
        config.NewConfig<ValidationFailure, ValidationErrorDto>();

        config.Compile();
    }
}

