using Domain.Models;

namespace Application.Abstractions.Services.Auth;

public interface IAccessTokenService
{
    public string GenerateAccessToken(User user, IList<string> userRoles);
}
