using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(AuthRequestDto authRequestDto);
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);
    }
}