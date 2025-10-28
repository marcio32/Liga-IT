using Liga_IT.Domain.Enums;

namespace Liga_IT.Domain.Entities;

public class Referee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public RefereeCategoryEnum Category { get; set; } = RefereeCategoryEnum.National;
    public ICollection<Match> Matches { get; set; } = new List<Match>();
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}

