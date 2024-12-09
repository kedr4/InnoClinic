using Application.DTOs;

namespace Application.Abstractions.Services;

public interface IJwtTokenService
{
    public string GenerateJwtToken(Guid userId, RolesEnum role);
    public bool ValidateJwtToken(string jwtToken);
}
