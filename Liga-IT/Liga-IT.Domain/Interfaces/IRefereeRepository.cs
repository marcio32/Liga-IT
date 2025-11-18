using Liga_IT.Domain.Entities;

namespace Liga_IT.Domain.Interfaces;

public interface IRefereeRepository
{
    Task<Referee> AddAsync(Referee referee);
    Task<bool> DeleteAsync(Referee referee);
    Task<IEnumerable<Referee>> GetAllAsync();
    Task<Referee?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(Referee referee);
}
