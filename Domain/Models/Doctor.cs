using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Doctor : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public DateTimeOffset DateOfBirth { get; set; }
    public Guid AccountId { get; set; }
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public int CareerStartYear { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}