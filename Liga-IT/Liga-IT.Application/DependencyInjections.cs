using Liga_IT.Application.Interfaces;
using Liga_IT.Application.Mappings;
using Liga_IT.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Liga_IT.Application;

public static class DependencyInjections
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Mapster
        MappingConfig.RegisterMappings();

        //Services 
        services.AddScoped<IClubService, ClubService>();

        return services;
    }
}
