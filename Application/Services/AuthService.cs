using Application.Abstrsctions.Services;
using Application.DTOs;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Exceptions;
using Application.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;


namespace Application.Services;

public class AuthService
    (
    UserManager<Doctor> doctorManager,
    UserManager<Patient> patientManager,
    UserManager<Receptionist> receptionistManager,
    IRefreshTokenService refreshTokenService,
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

        string jwtToken = jwtTokenService.GenerateJwtToken(loginResult.Item2, role);
        string refreshToken = refreshTokenService.GenerateRefreshToken(loginResult.Item2).Token.ToString();

        return new LoginResponse { UserId = loginResult.Item2, JwtToken = jwtToken, RefreshToken = refreshToken };

    }


    public async Task LogoutAsync(Guid userId, string refreshToken)
    {
        bool result = await refreshTokenService.ValidateRefreshToken(userId, refreshToken);

        if (!result)
        {
            throw new InvalidRefreshTokenException(refreshToken);
        }

        await refreshTokenService.RevokeRefreshToken(userId, refreshToken);
    }

    public async Task DeleteProfileAsync(Guid userId, RolesEnum role)
    {

        switch (role)
        {
            case RolesEnum.Patient:

                var patient = await patientManager.FindByIdAsync(userId.ToString());

                if (patient == null)
                {
                    throw new UserNotFoundException(userId);
                }

                var resultFormPatient = await patientManager.DeleteAsync(patient);
                ErrorCaster.CheckForUserNotFoundException(resultFormPatient, userId);

                break;

            case RolesEnum.Doctor:

                var doctor = await doctorManager.FindByIdAsync(userId.ToString());

                if (doctor == null)
                {
                    throw new UserNotFoundException(userId);
                }

                var resultFromDoctor = await doctorManager.DeleteAsync(doctor);
                ErrorCaster.CheckForUserNotFoundException(resultFromDoctor, userId);

                break;

            case RolesEnum.Receptionist:

                var receptionist = await receptionistManager.FindByIdAsync(userId.ToString());

                if (receptionist == null)
                {
                    throw new UserNotFoundException(userId);
                }

                var resultFromReceptionist = await receptionistManager.DeleteAsync(receptionist);
                ErrorCaster.CheckForUserNotFoundException(resultFromReceptionist, userId);

                break;
        }
    }

    public async Task<string> RefreshAccessTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
