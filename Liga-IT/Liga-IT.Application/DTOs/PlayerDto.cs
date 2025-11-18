namespace Liga_IT.Application.DTOs;

public class PlayerDto
{
    public int Id { get; set; }
    public int Age { get; set; }
    public int JerseyNumber { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int MatchesPlayed { get; set; }
    public int ClubId { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime JoinedClubDate { get; set; }
    public DateTime CreateAt { get; set; }
}

public class AddPlayerDto
{
    public int Age { get; set; }
    public int JerseyNumber { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public int ClubId { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime JoinedClubDate { get; set; }
}

public class UpdatePlayerDto
{
    public int Id { get; set; }
    public int Age { get; set; }
    public int JerseyNumber { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int MatchesPlayed { get; set; }
    public int ClubId { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime JoinedClubDate { get; set; }
}
