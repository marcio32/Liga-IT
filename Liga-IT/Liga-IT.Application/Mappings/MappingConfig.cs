using Liga_IT.Application.DTOs;
using Liga_IT.Domain.Entities;
using Mapster;

namespace Liga_IT.Application.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<Club, ClubDto>();
        config.NewConfig<AddClubDto, Club>()
            .Map(dest => dest.CreateAt, src => DateTime.UtcNow);

        config.Compile();
    }
}

