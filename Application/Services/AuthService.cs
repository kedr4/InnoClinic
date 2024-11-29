using Application.Abstrsctions.Services;
using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Exceptions;
using Application.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService(UserManager<Doctor> doctorManager,
UserManager<Patient> patientManager,
UserManager<Receptionist> receptionistManager,
IRefreshTokenService refreshTokenGenerator) : IAuthService
{

    public async Task<CreatePatientResponse> RegisterPatientAsync(CreatePatientRequest request)
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

        return new CreatePatientResponse
        {
            Success = result.Succeeded,
            Message = result.Succeeded ? "Registration successful" : "Registration failed",
            UserId = user.Id
        };
    }



    public async Task<CreateDoctorResponse> RegisterDoctorAsync(CreateDoctorRequest request)
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

        return new CreateDoctorResponse
        {
            Success = result.Succeeded,
            Message = result.Succeeded ? "Doctor registered successfully." : "Failed to register doctor.",
            UserId = user.Id
        };
    }


    public async Task<CreateReceptionistResponse> RegisterReceptionistAsync(CreateReceptionistRequest request)
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

        return new CreateReceptionistResponse
        {
            Success = result.Succeeded,
            Message = result.Succeeded ? "Receptionist registered successfully." : "Failed to register receptionist.",
            ReceptionistId = user.Id
        };
    }


    public async Task<LoginResponse> LoginAsync(LoginRequest request, RolesEnum role)
    {
        Tuple<bool, Guid> loginResult;
        bool checkLoginResult = false;

        switch (role)
        {
            case RolesEnum.Doctor:
                var doctor = await doctorManager.FindByEmailAsync(request.Email);

                if (doctor == null)
                {
                    throw new InvalidLoginException();
                }

                checkLoginResult = await doctorManager.CheckPasswordAsync(doctor, request.Password);
                loginResult = new Tuple<bool, Guid>(checkLoginResult, doctor.Id);
                ErrorCaster.CheckForInvalidLoginException(checkLoginResult);

                break;

            case RolesEnum.Patient:
                var patient = await patientManager.FindByEmailAsync(request.Email);

                if (patient  is null)
                {
                    throw new InvalidLoginException();
                }

                checkLoginResult = await patientManager.CheckPasswordAsync(patient, request.Password);
                loginResult = new Tuple<bool, Guid>(checkLoginResult, patient.Id);
                ErrorCaster.CheckForInvalidLoginException(checkLoginResult);

                break;

            case RolesEnum.Receptionist:
                var receptionist = await receptionistManager.FindByEmailAsync(request.Email);

                if (receptionist == null)
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

        //generate jwt
        // generate refresh token
        return new LoginResponse { UserId = loginResult.Item2, JwtToken = "testjwt", RefreshToken = "testrefresh" };

    }


    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var result = await refreshTokenGenerator.ValidateRefreshTokenAsync(refreshToken);

        if (!result)
        {
            throw new InvalidRefreshTokenException(refreshToken);
        }

        await refreshTokenGenerator.RevokeRefreshTokenAsync(refreshToken);

        return true; //TODO: some more logic
    }


    public async Task DeleteDoctorProfileAsync(Guid userId)
    {
        var user = await doctorManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var result = await doctorManager.DeleteAsync(user);
        ErrorCaster.CheckForUserNotFoundException(result, userId);
    }

    public async Task DeletePatientProfileAsync(Guid userId)
    {
        var user = await patientManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var result = await patientManager.DeleteAsync(user);
        ErrorCaster.CheckForUserNotFoundException(result, userId);
    }

    public async Task DeleteReceptionistProfileAsync(Guid userId)
    {
        var user = await receptionistManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var result = await receptionistManager.DeleteAsync(user);
        ErrorCaster.CheckForUserNotFoundException(result, userId);
    }

    public async Task<string> RefreshAccessTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
