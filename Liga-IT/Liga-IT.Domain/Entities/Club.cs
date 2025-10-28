namespace Liga_IT.Domain.Entities;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int NumberOfPartners { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string StadiumName { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }

    //Navegacional properties
    public IEnumerable<Player> Players { get; set; } = new List<Player>();
    public IEnumerable<Match> HomeMatches { get; set; } = new List<Match>();
    public IEnumerable<Match> AwayMatches { get; set; } = new List<Match>();

}

