using Application.DTOs;

namespace Application.Abstrsctions.Services;

public interface IJwtTokenService
{
    public string GenerateJwtToken(Guid userId, RolesEnum role);
    public bool ValidateJwtToken(string jwtToken);
}
