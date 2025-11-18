using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Interfaces;

public interface IMatchService
{
    Task<MatchDto> CreateMatchAsync(AddMatchDto addMatchDto);
    Task<bool> DeleteMatchAsync(int id);
    Task<IEnumerable<MatchDto>> GetAllMatchesAsync();
    Task<MatchDto?> GetMatchByIdAsync(int id);
    Task<IEnumerable<MatchDto>> GetMatchesByClubAsync(int clubId);
    Task<IEnumerable<MatchDto>> GetMatchesByRoundAsync(int round);
    Task<bool> UpdateMatchAsync(UpdateMatchDto updateMatchDto);
}
