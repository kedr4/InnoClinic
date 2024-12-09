using Microsoft.AspNetCore.Identity;

namespace Domain.Models;
public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public DateTimeOffset DateOfBirth { get; set; }
    public Guid AccountId { get; set; }
}
