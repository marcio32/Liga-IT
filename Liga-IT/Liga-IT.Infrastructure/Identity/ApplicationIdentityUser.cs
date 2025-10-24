using Microsoft.AspNetCore.Identity;

namespace Liga_IT.Infrastructure.Identity;

public class ApplicationIdentityUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } =  string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
