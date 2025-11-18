using Amazon.SQS;
using Liga_IT.Application.Interfaces;
using Liga_IT.Domain.Interfaces;
using Liga_IT.Infrastructure.Data;
using Liga_IT.Infrastructure.Identity;
using Liga_IT.Infrastructure.Repositories;
using Liga_IT.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Liga_IT.Infrastructure;

public static class DependecyInjections
{
    public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration configuration)
    {
        //Database
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        //Redis cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "LigaIT";
        });

        //AWS SQS
        services.AddSingleton<IAmazonSQS>(options =>
        {
            var config = new AmazonSQSConfig
            {
                //ServiceURL = configuration.GetSection("AWS:ServiceURL").Value,
                //UseHttp = bool.Parse(configuration.GetSection("AWS:UseHttp").Value!)
                RegionEndpoint = Amazon.RegionEndpoint.USEast1
            };
            return new AmazonSQSClient(configuration.GetSection("AWS:AccessKey").Value, configuration.GetSection("AWS:SecretKey").Value, config);
        });

        services.AddIdentity<ApplicationIdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        //Servicios
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClubRepository, ClubRepository>();
        services.AddScoped<IRefereeRepository, RefereeRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<ISqsService, SqsService>();

        return services;
    }
}

