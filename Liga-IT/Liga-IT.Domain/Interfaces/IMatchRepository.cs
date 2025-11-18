using Liga_IT.Domain.Entities;

namespace Liga_IT.Domain.Interfaces;

public interface IMatchRepository
{
    Task<Match> AddAsync(Match match);
    Task<bool> DeleteAsync(Match match);
    Task<IEnumerable<Match>> GetAllAsync();
    Task<Match?> GetByIdAsync(int id);
    Task<IEnumerable<Match>> GetByClubIdAsync(int clubId);
    Task<IEnumerable<Match>> GetByRoundAsync(int round);
    Task<bool> UpdateAsync(Match match);
}
