using Liga_IT.Domain.Entities;
using Liga_IT.Domain.Interfaces;
using Liga_IT.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Liga_IT.Infrastructure.Repositories;

public class MatchRepository(ApplicationDbContext applicationDbContext) : IMatchRepository
{
    public async Task<IEnumerable<Match>> GetAllAsync()
    {
        return await applicationDbContext.Match
            .Include(m => m.HomeClub)
            .Include(m => m.AwayClub)
            .Include(m => m.Referee)
            .ToListAsync();
    }

    public async Task<Match?> GetByIdAsync(int id)
    {
        return await applicationDbContext.Match
            .Include(m => m.HomeClub)
            .Include(m => m.AwayClub)
            .Include(m => m.Referee)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Match>> GetByClubIdAsync(int clubId)
    {
        return await applicationDbContext.Match
            .Include(m => m.HomeClub)
            .Include(m => m.AwayClub)
            .Include(m => m.Referee)
            .Where(m => m.HomeClubId == clubId || m.AwayClubId == clubId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Match>> GetByRoundAsync(int round)
    {
        return await applicationDbContext.Match
            .Include(m => m.HomeClub)
            .Include(m => m.AwayClub)
            .Include(m => m.Referee)
            .Where(m => m.Round == round)
            .ToListAsync();
    }

    public async Task<Match> AddAsync(Match match)
    {
        await applicationDbContext.Match.AddAsync(match);
        await applicationDbContext.SaveChangesAsync();
        return match;
    }

    public async Task<bool> UpdateAsync(Match match)
    {
        applicationDbContext.Match.Update(match);
        return await applicationDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Match match)
    {
        applicationDbContext.Match.Remove(match);
        return await applicationDbContext.SaveChangesAsync() > 0;
    }
}
