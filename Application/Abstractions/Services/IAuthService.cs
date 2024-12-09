using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Abstractions.Services;

public interface IAuthService
{
    public Task<Guid> RegisterPatientAsync(CreatePatientRequest request);
    public Task<Guid> RegisterDoctorAsync(CreateDoctorRequest request);
    public Task<Guid> RegisterReceptionistAsync(CreateReceptionistRequest request);
    public Task<LoginResponse> LoginAsync(LoginRequest request);
    public Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken);
    public Task DeleteProfileAsync(DeleteProfileRequest request);
    public Task<string> RefreshAccessTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
}
