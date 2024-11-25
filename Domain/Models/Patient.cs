using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Patient : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public bool isLinkedToAccount { get; set; }
    public DateOnly DateOfBirth { get; set; }
    //
    public Guid AccountId { get; set; }
}
