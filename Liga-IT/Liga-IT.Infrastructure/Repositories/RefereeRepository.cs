using Liga_IT.Domain.Entities;
using Liga_IT.Domain.Interfaces;
using Liga_IT.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Liga_IT.Infrastructure.Repositories;

public class RefereeRepository(ApplicationDbContext applicationDbContext) : IRefereeRepository
{
    public async Task<IEnumerable<Referee>> GetAllAsync()
    {
        return await applicationDbContext.Referee.ToListAsync();
    }

    public async Task<Referee?> GetByIdAsync(int id)
    {
        return await applicationDbContext.Referee.FindAsync(id);
    }

    public async Task<Referee> AddAsync(Referee referee)
    {
        await applicationDbContext.Referee.AddAsync(referee);
        await applicationDbContext.SaveChangesAsync();
        return referee;
    }

    public async Task<bool> UpdateAsync(Referee referee)
    {
        applicationDbContext.Referee.Update(referee);
        return await applicationDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Referee referee)
    {
        applicationDbContext.Referee.Remove(referee);
        return await applicationDbContext.SaveChangesAsync() > 0;
    }
}
