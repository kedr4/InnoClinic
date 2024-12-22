namespace Application.Abstractions.Services.Auth;

public interface IRolesService
{
    public Task SetRolesAsync(CancellationToken cancellationToken);
}
