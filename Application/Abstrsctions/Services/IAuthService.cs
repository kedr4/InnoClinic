namespace Application.Abstrsctions.Services;

public interface IAuthService
{
    Task RegisterPatientAsync(string email, string password, CancellationToken cancellationToken = default);
    Task RegisterDoctorAsync(string email, string password, CancellationToken cancellationToken = default);
    Task RegisterReceptionistAsync(string email, string password, CancellationToken cancellationToken = default);

    Task LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task LogoutAsync(CancellationToken cancellationToken = default);
    Task DeleteProfileAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<string> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
