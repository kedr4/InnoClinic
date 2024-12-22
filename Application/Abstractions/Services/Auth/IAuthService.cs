using Application.Abstractions.DTOs;
using Domain.Models;

namespace Application.Abstractions.Services.Auth;

public interface IAuthService
{
    public Task<Guid> RegisterUserAsync(CreateUserRequest request, Roles role, CancellationToken cancellationToken);
    public Task<LoginUserResponse> LoginUserAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken);
    public Task<bool> LogoutUserAsync(LogoutUserRequest request, CancellationToken cancellationToken);
    public Task ConfirmMailAsync(ConfirmMailRequest request, CancellationToken cancellationToken);
}
