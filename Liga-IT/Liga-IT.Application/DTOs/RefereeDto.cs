using Liga_IT.Domain.Enums;

namespace Liga_IT.Application.DTOs;

public class RefereeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public RefereeCategoryEnum Category { get; set; }
    public DateTime CreateAt { get; set; }
}

public class AddRefereeDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public RefereeCategoryEnum Category { get; set; }
}

public class UpdateRefereeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public RefereeCategoryEnum Category { get; set; }
}
