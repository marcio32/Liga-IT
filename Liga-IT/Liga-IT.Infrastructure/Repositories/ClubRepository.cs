using Liga_IT.Domain.Entities;
using Liga_IT.Domain.Interfaces;
using Liga_IT.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Liga_IT.Infrastructure.Repositories;

public class ClubRepository(ApplicationDbContext applicationDbContext) : IClubRepository
{
    public async Task<IEnumerable<Club>> GetAllAsync()
    {
        return await applicationDbContext.Club.ToListAsync();
    }

    public async Task<Club?> GetByIdAsync(int id)
    {
        return await applicationDbContext.Club.FindAsync(id);
    }

    public async Task<Club> AddAsync(Club club)
    {
        await applicationDbContext.Club.AddAsync(club);
        await applicationDbContext.SaveChangesAsync();
        return club;
    }

    public async Task<bool> UpdateAsync(Club club)
    {
        applicationDbContext.Club.Update(club);
        return await applicationDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Club club)
    {
        applicationDbContext.Club.Remove(club);
        return await applicationDbContext.SaveChangesAsync() > 0;
    }




}
