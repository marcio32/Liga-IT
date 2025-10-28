using Liga_IT.Domain.Enums;

namespace Liga_IT.Domain.Entities;

public class Match
{
    public int Id { get; set; }
    public int? HomeScore { get; set; }
    public int? AwayScore { get; set; }
    public int Round { get; set; }
    public int HomeClubId { get; set; }
    public int AwayClubId { get; set; }

    public string Notes { get; set; } = string.Empty;
    public int? RefereeId { get; set; }
    public Referee? Referee { get; set; }
    public Club HomeClub { get; set; } = null!;
    public Club AwayClub { get; set; } = null!;
    public MatchStatusEnum Status { get; set; } = MatchStatusEnum.Scheduled;
    public DateTime MatchDate { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }


}

