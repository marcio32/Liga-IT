using Liga_IT.Application.DTOs;
using Liga_IT.Application.Interfaces;
using Liga_IT.Domain.Entities;
using Liga_IT.Domain.Interfaces;
using Mapster;

namespace Liga_IT.Application.Services;

public class PlayerService(IPlayerRepository playerRepository, IRedisCacheService cacheService, ISqsService sqsService) : IPlayerService
{
    public async Task<IEnumerable<PlayerDto>> GetAllPlayersAsync()
    {
        var cachedPlayers = await cacheService.GetAsync<IEnumerable<PlayerDto>>("players:all");

        if (cachedPlayers != null)
            return cachedPlayers;

        var players = await playerRepository.GetAllAsync();

        await cacheService.SetAsync("players:all", players.Select(MapToDto));

        return players.Select(MapToDto);
    }

    public async Task<PlayerDto?> GetPlayerByIdAsync(int id)
    {
        var player = await playerRepository.GetByIdAsync(id);
        return player != null ? MapToDto(player) : null;
    }

    public async Task<IEnumerable<PlayerDto>> GetPlayersByClubAsync(int clubId)
    {
        var players = await playerRepository.GetByClubIdAsync(clubId);
        return players.Select(MapToDto);
    }

    public async Task<PlayerDto> CreatePlayerAsync(AddPlayerDto addPlayerDto)
    {
        await cacheService.RemoveAsync("players:all");
        var player = addPlayerDto.Adapt<Player>();
        player.CreateAt = DateTime.UtcNow;
        player.UpdateAt = DateTime.UtcNow;
        var createdPlayer = await playerRepository.AddAsync(player);
        await sqsService.SendMessageAsync(new
        {
            EventType = "PlayerCreated",
            createdPlayer,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.PlayerQueue);
        return MapToDto(createdPlayer);
    }

    public async Task<bool> UpdatePlayerAsync(UpdatePlayerDto updatePlayerDto)
    {
        await cacheService.RemoveAsync("players:all");

        await sqsService.SendMessageAsync(new
        {
            EventType = "PlayerUpdated",
            updatePlayerDto,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.PlayerQueue);

        var existingPlayer = await playerRepository.GetByIdAsync(updatePlayerDto.Id);
        if (existingPlayer == null)
            return false;
        updatePlayerDto.Adapt(existingPlayer);
        existingPlayer.UpdateAt = DateTime.UtcNow;
        return await playerRepository.UpdateAsync(existingPlayer);
    }

    public async Task<bool> DeletePlayerAsync(int id)
    {
        await cacheService.RemoveAsync("players:all");
        var existingPlayer = await playerRepository.GetByIdAsync(id);
        if (existingPlayer == null)
            return false;

        await sqsService.SendMessageAsync(new
        {
            EventType = "PlayerDeleted",
            Id = id,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.PlayerQueue);
        return await playerRepository.DeleteAsync(existingPlayer);
    }

    private static PlayerDto MapToDto(Player player) => player.Adapt<PlayerDto>();
}
