using Liga_IT.Application.DTOs;
using Liga_IT.Application.Interfaces;
using Liga_IT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Liga_IT.Infrastructure.Services;

public class AuthService(UserManager<ApplicationIdentityUser> userManager, IConfiguration configuration) : IAuthService
{

    public async Task<AuthResponseDto> LoginAsync(AuthRequestDto authRequestDto)
    {
        var user = await userManager.FindByEmailAsync(authRequestDto.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Email y password invalidos.");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, authRequestDto.Password);
        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Email y password invalidos.");

        var token = await GenerateJwtTokenAsync(user);

        return new AuthResponseDto
        {
            Token = token
        };
    }

    public async Task<string> GenerateJwtTokenAsync(ApplicationIdentityUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

