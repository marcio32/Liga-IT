using Liga_IT.Domain.Entities;
using Liga_IT.Domain.Interfaces;
using Liga_IT.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Liga_IT.Infrastructure.Repositories;

public class PlayerRepository(ApplicationDbContext applicationDbContext) : IPlayerRepository
{
    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        return await applicationDbContext.Player
            .Include(p => p.Club)
            .ToListAsync();
    }

    public async Task<Player?> GetByIdAsync(int id)
    {
        return await applicationDbContext.Player
            .Include(p => p.Club)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Player>> GetByClubIdAsync(int clubId)
    {
        return await applicationDbContext.Player
            .Include(p => p.Club)
            .Where(p => p.ClubId == clubId)
            .ToListAsync();
    }

    public async Task<Player> AddAsync(Player player)
    {
        await applicationDbContext.Player.AddAsync(player);
        await applicationDbContext.SaveChangesAsync();
        return player;
    }

    public async Task<bool> UpdateAsync(Player player)
    {
        applicationDbContext.Player.Update(player);
        return await applicationDbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Player player)
    {
        applicationDbContext.Player.Remove(player);
        return await applicationDbContext.SaveChangesAsync() > 0;
    }
}
