using Application.Abstractions.Persistance.Repositories;
using Application.Abstractions.Services;
using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Exceptions;
using Application.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class AuthService
    (
    UserManager<Doctor> doctorManager,
    UserManager<Patient> patientManager,
    UserManager<Receptionist> receptionistManager,
    IRefreshTokenService refreshTokenService,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenService jwtTokenService
    ) : IAuthService
{

    public async Task<Guid> RegisterPatientAsync(CreatePatientRequest request)
    {
        var user = new Patient
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            DateOfBirth = request.DateOfBirth,
            isLinkedToAccount = request.IsLinkedToAccount,
            AccountId = Guid.NewGuid()
        };

        var result = await patientManager.CreateAsync(user, request.Password);

        ErrorCaster.CheckForUserRegistrationException(result);

        return user.Id;
    }

    public async Task<Guid> RegisterDoctorAsync(CreateDoctorRequest request)
    {

        var user = new Doctor
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            DateOfBirth = request.DateOfBirth,
            SpecializationId = request.SpecializationId,
            OfficeId = request.OfficeId,
            CareerStartYear = request.CareerStartYear
        };

        var result = await doctorManager.CreateAsync(user, request.Password);

        ErrorCaster.CheckForUserRegistrationException(result);

        return user.Id;
    }

    public async Task<Guid> RegisterReceptionistAsync(CreateReceptionistRequest request)
    {
        var user = new Receptionist
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            DateOfBirth = request.DateOfBirth,
            AccountId = request.AccountId,
            OfficeId = request.OfficeId
        };

        var result = await receptionistManager.CreateAsync(user, request.Password);
        ErrorCaster.CheckForUserRegistrationException(result);

        return user.Id;
    }


    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        Tuple<bool, Guid> loginResult;
        bool checkLoginResult = false;

        switch (request.Role)
        {
            case RolesEnum.Doctor:
                var doctor = await doctorManager.FindByEmailAsync(request.Email);

                if (doctor is null)
                {
                    throw new InvalidLoginException();
                }

                checkLoginResult = await doctorManager.CheckPasswordAsync(doctor, request.Password);
                loginResult = new Tuple<bool, Guid>(checkLoginResult, doctor.Id);
                ErrorCaster.CheckForInvalidLoginException(checkLoginResult);

                break;

            case RolesEnum.Patient:
                var patient = await patientManager.FindByEmailAsync(request.Email);

                if (patient is null)
                {
                    throw new InvalidLoginException();
                }

                checkLoginResult = await patientManager.CheckPasswordAsync(patient, request.Password);
                loginResult = new Tuple<bool, Guid>(checkLoginResult, patient.Id);
                ErrorCaster.CheckForInvalidLoginException(checkLoginResult);

                break;

            case RolesEnum.Receptionist:
                var receptionist = await receptionistManager.FindByEmailAsync(request.Email);

                if (receptionist is null)
                {
                    throw new InvalidLoginException();
                }

                checkLoginResult = await receptionistManager.CheckPasswordAsync(receptionist, request.Password);
                loginResult = new Tuple<bool, Guid>(checkLoginResult, receptionist.Id);
                ErrorCaster.CheckForInvalidLoginException(checkLoginResult);

                break;

            default:
                throw new InvalidLoginException();
        }

        string jwtToken = jwtTokenService.GenerateJwtToken(loginResult.Item2, request.Role);
        string refreshToken = refreshTokenService.GenerateRefreshToken(loginResult.Item2).Token.ToString();

        return new LoginResponse(loginResult.Item2, jwtToken, refreshToken);
    }


    public async Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
    {
        bool result = await refreshTokenService.ValidateRefreshToken(request.UserId, request.RefreshToken, cancellationToken);

        if (!result)
        {
            throw new InvalidRefreshTokenException(request.RefreshToken);
        }

        await refreshTokenService.RevokeRefreshToken(request.UserId, request.RefreshToken, cancellationToken);
    }

    public async Task DeleteProfileAsync(DeleteProfileRequest request)
    {
        Tuple<bool, Guid> loginResult;
        bool checkLoginResult = false;

        switch (request.Role)
        {
            case RolesEnum.Patient:

                var patient = await patientManager.FindByIdAsync(request.UserId.ToString());

                if (patient is null)
                {
                    throw new UserNotFoundException(request.UserId);
                }

                checkLoginResult = await patientManager.CheckPasswordAsync(patient, request.Password);
                loginResult = new Tuple<bool, Guid>(checkLoginResult, patient.Id);
                ErrorCaster.CheckForInvalidLoginException(checkLoginResult);

                if (loginResult.Item1)
                {
                    var resultFormPatient = await patientManager.DeleteAsync(patient);
                    ErrorCaster.CheckForUserNotFoundException(resultFormPatient, request.UserId);
                }

                break;

            case RolesEnum.Doctor:

                var doctor = await doctorManager.FindByIdAsync(request.UserId.ToString());

                if (doctor is null)
                {
                    throw new UserNotFoundException(request.UserId);
                }

                checkLoginResult = await doctorManager.CheckPasswordAsync(doctor, request.Password);
                loginResult = new Tuple<bool, Guid>(checkLoginResult, doctor.Id);
                ErrorCaster.CheckForInvalidLoginException(checkLoginResult);

                if (loginResult.Item1)
                {
                    var resultFromDoctor = await doctorManager.DeleteAsync(doctor);
                    ErrorCaster.CheckForUserNotFoundException(resultFromDoctor, request.UserId);
                }

                break;

            case RolesEnum.Receptionist:

                var receptionist = await receptionistManager.FindByIdAsync(request.UserId.ToString());

                if (receptionist is null)
                {
                    throw new UserNotFoundException(request.UserId);
                }

                checkLoginResult = await receptionistManager.CheckPasswordAsync(receptionist, request.Password);
                loginResult = new Tuple<bool, Guid>(checkLoginResult, receptionist.Id);

                if (loginResult.Item1)
                {
                    var resultFromReceptionist = await receptionistManager.DeleteAsync(receptionist);
                    ErrorCaster.CheckForUserNotFoundException(resultFromReceptionist, request.UserId);
                }

                break;
        }
    }

    public async Task<string> RefreshAccessTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        if (request == null || string.IsNullOrEmpty(request.RefreshToken))
        {
            throw new ArgumentException("Invalid refresh token request");
        }

        var isValid = await refreshTokenService.ValidateRefreshToken(request.UserId, request.RefreshToken, cancellationToken);

        if (!isValid)
        {
            throw new SecurityTokenException("Refresh token is invalid or expired");
        }

        await refreshTokenService.RevokeRefreshToken(request.UserId, request.RefreshToken, cancellationToken);
        var newRefreshToken = refreshTokenService.GenerateRefreshToken(request.UserId);
        await refreshTokenRepository.AddAsync(newRefreshToken);
        await refreshTokenService.SetRefreshToken(request.UserId, newRefreshToken, cancellationToken);

        return newRefreshToken.Token;
    }
}
