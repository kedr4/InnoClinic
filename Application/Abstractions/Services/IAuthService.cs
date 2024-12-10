using Application.Abstractions.DTOs;
using Domain.Models;

namespace Application.Abstractions.Services;

public interface IAuthService
{
    public Task<Guid> RegisterUserAsync(string email, string password, Roles role, CancellationToken cancellationToken);
    public Task<bool> CheckUserPasswordAsync(string email, string password);
    public Task<LoginUserResponse> LoginUserAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken);
    public Task<bool> LogoutUserAsync(LogoutUserRequest request, CancellationToken cancellationToken);
}
