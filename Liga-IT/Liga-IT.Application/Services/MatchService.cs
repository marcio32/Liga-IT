using Liga_IT.Application.DTOs;
using Liga_IT.Application.Interfaces;
using Liga_IT.Domain.Entities;
using Liga_IT.Domain.Interfaces;
using Mapster;

namespace Liga_IT.Application.Services;

public class MatchService(IMatchRepository matchRepository, IRedisCacheService cacheService, ISqsService sqsService) : IMatchService
{
    public async Task<IEnumerable<MatchDto>> GetAllMatchesAsync()
    {
        var cachedMatches = await cacheService.GetAsync<IEnumerable<MatchDto>>("matches:all");

        if (cachedMatches != null)
            return cachedMatches;

        var matches = await matchRepository.GetAllAsync();

        await cacheService.SetAsync("matches:all", matches.Select(MapToDto));

        return matches.Select(MapToDto);
    }

    public async Task<MatchDto?> GetMatchByIdAsync(int id)
    {
        var match = await matchRepository.GetByIdAsync(id);
        return match != null ? MapToDto(match) : null;
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByClubAsync(int clubId)
    {
        var matches = await matchRepository.GetByClubIdAsync(clubId);
        return matches.Select(MapToDto);
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByRoundAsync(int round)
    {
        var matches = await matchRepository.GetByRoundAsync(round);
        return matches.Select(MapToDto);
    }

    public async Task<MatchDto> CreateMatchAsync(AddMatchDto addMatchDto)
    {
        await cacheService.RemoveAsync("matches:all");
        var match = addMatchDto.Adapt<Match>();
        match.CreateAt = DateTime.UtcNow;
        match.UpdateAt = DateTime.UtcNow;
        var createdMatch = await matchRepository.AddAsync(match);
        await sqsService.SendMessageAsync(new
        {
            EventType = "MatchCreated",
            createdMatch,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.MatchQueue);
        return MapToDto(createdMatch);
    }

    public async Task<bool> UpdateMatchAsync(UpdateMatchDto updateMatchDto)
    {
        await cacheService.RemoveAsync("matches:all");

        await sqsService.SendMessageAsync(new
        {
            EventType = "MatchUpdated",
            updateMatchDto,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.MatchQueue);

        var existingMatch = await matchRepository.GetByIdAsync(updateMatchDto.Id);
        if (existingMatch == null)
            return false;
        updateMatchDto.Adapt(existingMatch);
        existingMatch.UpdateAt = DateTime.UtcNow;
        return await matchRepository.UpdateAsync(existingMatch);
    }

    public async Task<bool> DeleteMatchAsync(int id)
    {
        await cacheService.RemoveAsync("matches:all");
        var existingMatch = await matchRepository.GetByIdAsync(id);
        if (existingMatch == null)
            return false;

        await sqsService.SendMessageAsync(new
        {
            EventType = "MatchDeleted",
            Id = id,
            TimeStamp = DateTime.UtcNow
        }, QueueNames.MatchQueue);
        return await matchRepository.DeleteAsync(existingMatch);
    }

    private static MatchDto MapToDto(Match match) => match.Adapt<MatchDto>();
}
