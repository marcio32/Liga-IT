namespace Liga_IT.Application.DTOs;

public class ClubDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int NumberOfPartners { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string StadiumName { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}

public class AddClubDto
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int NumberOfPartners { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string StadiumName { get; set; } = string.Empty;
}

public class UpdateClubDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int NumberOfPartners { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string StadiumName { get; set; } = string.Empty;
}

