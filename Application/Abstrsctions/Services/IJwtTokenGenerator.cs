namespace Application.Abstrsctions.Services;
public interface IJwtTokenGenerator
{
    public Task<string> GenerateJwtTokenAsync(Guid userId);
    public Task<bool> ValidateJwtTokenAsync(string jwtToken);
}
