using Liga_IT.Domain.Enums;

namespace Liga_IT.Application.DTOs;

public class MatchDto
{
    public int Id { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public int Round { get; set; }
    public int HomeClubId { get; set; }
    public int AwayClubId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int? RefereeId { get; set; }
    public MatchStatusEnum Status { get; set; }
    public DateTime MatchDate { get; set; }
    public DateTime CreateAt { get; set; }
}

public class AddMatchDto
{
    public int Round { get; set; }
    public int HomeClubId { get; set; }
    public int AwayClubId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int? RefereeId { get; set; }
    public MatchStatusEnum Status { get; set; }
    public DateTime MatchDate { get; set; }
}

public class UpdateMatchDto
{
    public int Id { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public int Round { get; set; }
    public int HomeClubId { get; set; }
    public int AwayClubId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int? RefereeId { get; set; }
    public MatchStatusEnum Status { get; set; }
    public DateTime MatchDate { get; set; }
}
