using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Interfaces;

public interface IPlayerService
{
    Task<PlayerDto> CreatePlayerAsync(AddPlayerDto addPlayerDto);
    Task<bool> DeletePlayerAsync(int id);
    Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();
    Task<PlayerDto?> GetPlayerByIdAsync(int id);
    Task<IEnumerable<PlayerDto>> GetPlayersByClubAsync(int clubId);
    Task<bool> UpdatePlayerAsync(UpdatePlayerDto updatePlayerDto);
}
