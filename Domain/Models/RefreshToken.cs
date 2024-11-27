using Microsoft.AspNetCore.Identity;

namespace Domain.Models;


public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public Guid UserId { get; set; }
    public IdentityUser<Guid> User { get; set; } = null!;
}

