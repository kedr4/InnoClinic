using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class User : IdentityUser<Guid>
{
    public RefreshToken? RefreshToken { get; set; }
}