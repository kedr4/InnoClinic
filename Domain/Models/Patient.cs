using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Patient : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public bool isLinkedToAccount { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Guid AccountId { get; set; }
}
