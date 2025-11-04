using Liga_IT.Domain.Entities;

namespace Liga_IT.Domain.Interfaces;

public interface IClubRepository
{
    Task<Club> AddAsync(Club club);
    Task<bool> DeleteAsync(Club club);
    Task<IEnumerable<Club>> GetAllAsync();
    Task<Club?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(Club club);
}
