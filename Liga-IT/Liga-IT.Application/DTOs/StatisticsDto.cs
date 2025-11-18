namespace Liga_IT.Application.DTOs;

public class LeagueStatisticsDto
{
    public int TotalClubs { get; set; }
    public int TotalPlayers { get; set; }
    public int TotalMatches { get; set; }
    public int CompletedMatches { get; set; }
    public int ScheduledMatches { get; set; }
    public int TotalGoals { get; set; }
    public int TotalYellowCards { get; set; }
    public int TotalRedCards { get; set; }
    public decimal AverageGoalsPerMatch { get; set; }
}

public class MatchStatisticsDto
{
    public int TotalMatches { get; set; }
    public int CompletedMatches { get; set; }
    public int ScheduledMatches { get; set; }
    public int InProgressMatches { get; set; }
    public int PostponedMatches { get; set; }
    public int CancelledMatches { get; set; }
    public int TotalGoals { get; set; }
    public decimal AverageGoalsPerMatch { get; set; }
    public MatchDto? HighestScoringMatch { get; set; }
}

public class PlayerStatisticsDto
{
    public int TotalPlayers { get; set; }
    public int ActivePlayers { get; set; }
    public int InactivePlayers { get; set; }
    public int TotalGoals { get; set; }
    public int TotalAssists { get; set; }
    public int TotalYellowCards { get; set; }
    public int TotalRedCards { get; set; }
    public PlayerDto? TopScorer { get; set; }
    public PlayerDto? TopAssister { get; set; }
    public PlayerDto? MostYellowCards { get; set; }
}
