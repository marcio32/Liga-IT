using FluentValidation;
using Liga_IT.Application.DTOs;
using Liga_IT.Application.Interfaces;
using Liga_IT.Application.Mappings;
using Liga_IT.Application.Services;
using Liga_IT.Application.Validators;
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
        services.AddScoped<IRefereeService, RefereeService>();
        services.AddScoped<IMatchService, MatchService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IStatisticsService, StatisticsService>();

        //Validators
        services.AddScoped<IValidator<AddClubDto>, AddClubValidator>();
        services.AddScoped<IValidator<UpdateClubDto>, UpdateClubValidator>();
        services.AddScoped<IValidator<AddPlayerDto>, AddPlayerValidator>();
        services.AddScoped<IValidator<UpdatePlayerDto>, UpdatePlayerValidator>();
        services.AddScoped<IValidator<AddRefereeDto>, AddRefereeValidator>();
        services.AddScoped<IValidator<UpdateRefereeDto>, UpdateRefereeValidator>();
        services.AddScoped<IValidator<AddMatchDto>, AddMatchValidator>();
        services.AddScoped<IValidator<UpdateMatchDto>, UpdateMatchValidator>();

        return services;
    }
}
