using Microsoft.AspNetCore.Identity;

namespace Domain.Models;


public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset ExpiresAt { get; set;} 
    public bool IsRevoked { get; set; }
    public Guid UserId { get; set; }
    public IdentityUser<Guid> User { get; set; } = null!;
}

