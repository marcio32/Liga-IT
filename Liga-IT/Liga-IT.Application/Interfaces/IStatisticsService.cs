using Liga_IT.Application.DTOs;

namespace Liga_IT.Application.Interfaces;

public interface IStatisticsService
{
    Task<LeagueStatisticsDto> GetLeagueStatisticsAsync();
    Task<MatchStatisticsDto> GetMatchStatisticsAsync();
    Task<PlayerStatisticsDto> GetPlayerStatisticsAsync();
}
