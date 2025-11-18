using Liga_IT.Application.DTOs;
using Liga_IT.Application.Interfaces;
using Liga_IT.Domain.Enums;
using Liga_IT.Domain.Interfaces;
using Mapster;

namespace Liga_IT.Application.Services;

public class StatisticsService(
    IClubRepository clubRepository,
    IPlayerRepository playerRepository,
    IMatchRepository matchRepository,
    IRedisCacheService cacheService) : IStatisticsService
{
    public async Task<LeagueStatisticsDto> GetLeagueStatisticsAsync()
    {
        var cached = await cacheService.GetAsync<LeagueStatisticsDto>("statistics:league");
        if (cached != null)
            return cached;

        var clubs = await clubRepository.GetAllAsync();
        var players = await playerRepository.GetAllAsync();
        var matches = await matchRepository.GetAllAsync();

        var completedMatches = matches.Where(m => m.Status == MatchStatusEnum.Completed).ToList();
        var totalGoals = completedMatches.Sum(m => (m.HomeScore ?? 0) + (m.AwayScore ?? 0));

        var stats = new LeagueStatisticsDto
        {
            TotalClubs = clubs.Count(),
            TotalPlayers = players.Count(),
            TotalMatches = matches.Count(),
            CompletedMatches = completedMatches.Count(),
            ScheduledMatches = matches.Count(m => m.Status == MatchStatusEnum.Scheduled),
            TotalGoals = totalGoals,
            TotalYellowCards = players.Sum(p => p.YellowCards),
            TotalRedCards = players.Sum(p => p.RedCards),
            AverageGoalsPerMatch = completedMatches.Any() ? (decimal)totalGoals / completedMatches.Count : 0
        };

        await cacheService.SetAsync("statistics:league", stats);
        return stats;
    }

    public async Task<MatchStatisticsDto> GetMatchStatisticsAsync()
    {
        var cached = await cacheService.GetAsync<MatchStatisticsDto>("statistics:matches");
        if (cached != null)
            return cached;

        var matches = await matchRepository.GetAllAsync();
        var completedMatches = matches.Where(m => m.Status == MatchStatusEnum.Completed).ToList();
        var totalGoals = completedMatches.Sum(m => (m.HomeScore ?? 0) + (m.AwayScore ?? 0));

        var highestScoring = completedMatches
            .OrderByDescending(m => (m.HomeScore ?? 0) + (m.AwayScore ?? 0))
            .FirstOrDefault();

        var stats = new MatchStatisticsDto
        {
            TotalMatches = matches.Count(),
            CompletedMatches = completedMatches.Count(),
            ScheduledMatches = matches.Count(m => m.Status == MatchStatusEnum.Scheduled),
            InProgressMatches = matches.Count(m => m.Status == MatchStatusEnum.InProgress),
            PostponedMatches = matches.Count(m => m.Status == MatchStatusEnum.Postponed),
            CancelledMatches = matches.Count(m => m.Status == MatchStatusEnum.Cancelled),
            TotalGoals = totalGoals,
            AverageGoalsPerMatch = completedMatches.Any() ? (decimal)totalGoals / completedMatches.Count : 0,
            HighestScoringMatch = highestScoring?.Adapt<MatchDto>()
        };

        await cacheService.SetAsync("statistics:matches", stats);
        return stats;
    }

    public async Task<PlayerStatisticsDto> GetPlayerStatisticsAsync()
    {
        var cached = await cacheService.GetAsync<PlayerStatisticsDto>("statistics:players");
        if (cached != null)
            return cached;

        var players = await playerRepository.GetAllAsync();
        var playersList = players.ToList();

        var topScorer = playersList.OrderByDescending(p => p.Goals).FirstOrDefault();
        var topAssister = playersList.OrderByDescending(p => p.Assists).FirstOrDefault();
        var mostYellowCards = playersList.OrderByDescending(p => p.YellowCards).FirstOrDefault();

        var stats = new PlayerStatisticsDto
        {
            TotalPlayers = playersList.Count(),
            ActivePlayers = playersList.Count(p => p.IsActive),
            InactivePlayers = playersList.Count(p => !p.IsActive),
            TotalGoals = playersList.Sum(p => p.Goals),
            TotalAssists = playersList.Sum(p => p.Assists),
            TotalYellowCards = playersList.Sum(p => p.YellowCards),
            TotalRedCards = playersList.Sum(p => p.RedCards),
            TopScorer = topScorer?.Adapt<PlayerDto>(),
            TopAssister = topAssister?.Adapt<PlayerDto>(),
            MostYellowCards = mostYellowCards?.Adapt<PlayerDto>()
        };

        await cacheService.SetAsync("statistics:players", stats);
        return stats;
    }
}
