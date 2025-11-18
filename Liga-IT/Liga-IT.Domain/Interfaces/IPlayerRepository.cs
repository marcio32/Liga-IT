using Liga_IT.Domain.Entities;

namespace Liga_IT.Domain.Interfaces;

public interface IPlayerRepository
{
    Task<Player> AddAsync(Player player);
    Task<bool> DeleteAsync(Player player);
    Task<IEnumerable<Player>> GetAllAsync();
    Task<Player?> GetByIdAsync(int id);
    Task<IEnumerable<Player>> GetByClubIdAsync(int clubId);
    Task<bool> UpdateAsync(Player player);
}
