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

public class AuthService(UserManager<ApplicationIdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : IAuthService
{

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        var existingUser = await userManager.FindByEmailAsync(registerRequestDto.Email);
        if (existingUser != null)
            throw new InvalidOperationException("El email ya está registrado.");

        var user = new ApplicationIdentityUser
        {
            Email = registerRequestDto.Email,
            UserName = registerRequestDto.Email,
            FirstName = registerRequestDto.FirstName,
            LastName = registerRequestDto.LastName
        };

        var result = await userManager.CreateAsync(user, registerRequestDto.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        var role = await roleManager.FindByIdAsync("1");
        if (role != null)
            await userManager.AddToRoleAsync(user, role.Name!);

        var token = await GenerateJwtTokenAsync(user);

        return new AuthResponseDto
        {
            Token = token
        };
    }

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
            expires: DateTime.UtcNow.AddYears(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

