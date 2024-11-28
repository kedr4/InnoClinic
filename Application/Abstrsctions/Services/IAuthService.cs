using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Abstrsctions.Services;

public interface IAuthService
{
    public Task<CreatePatientResponse> RegisterPatientAsync(CreatePatientRequest request);
    public Task<CreateDoctorResponse> RegisterDoctorAsync(CreateDoctorRequest request);
    public Task<CreateReceptionistResponse> RegisterReceptionistAsync(CreateReceptionistRequest request);
    public Task<LoginResponse> LoginAsync(LoginRequest request, RolesEnum roles);
    public Task<bool> LogoutAsync(string refreshToken);
    public Task<string> RefreshAccessTokenAsync(string refreshToken);
}
