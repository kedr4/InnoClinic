using Application.DTOs;

namespace Application.Abstrsctions.Services;
public interface IJwtTokenService
{
    public Task<string> GenerateJwtTokenAsync(Guid userId, RolesEnum role);
    public Task<bool> ValidateJwtTokenAsync(string jwtToken);
}
