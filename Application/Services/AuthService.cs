using Application.Abstrsctions.Services;
using Application.Exceptions;
using Application.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<Doctor> _doctorManager;
    private readonly UserManager<Patient> _patientManager;
    private readonly UserManager<Receptionist> _receptionistManager;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public AuthService(UserManager<Doctor> doctorManager,
    UserManager<Patient> patientManager,
    UserManager<Receptionist> receptionistManager,
    IRefreshTokenGenerator refreshTokenGenerator)
    {
        _doctorManager = doctorManager;
        _patientManager = patientManager;
        _receptionistManager = receptionistManager;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task RegisterPatientAsync(string email, string password, string firstName, string lastName, string middleName,
        DateTimeOffset dateOfBirth, bool isLinkedToAccount, Guid accountId)
    {
        var user = new Patient
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            DateOfBirth = dateOfBirth,
            isLinkedToAccount = isLinkedToAccount,
            AccountId = accountId
        };

        var result = await _patientManager.CreateAsync(user, password);
        ErrorCaster.CheckForUserRegistrationException(result);

    }


    public async Task RegisterDoctorAsync(string email, string password, string firstName, string lastName, string middleName,
        DateTimeOffset dateOfBirth, Guid accountId, Guid specializationId, Guid officeId, int careerStartYear)
    {
        var user = new Doctor
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            DateOfBirth = dateOfBirth,
            AccountId = accountId,
            SpecializationId = specializationId,
            OfficeId = officeId,
            CareerStartYear = careerStartYear
        };

        var result = await _doctorManager.CreateAsync(user, password);
        ErrorCaster.CheckForUserRegistrationException(result);
    }


    public async Task RegisterReceptionistAsync(string email, string password, string firstName, string lastName, string middleName,
        DateTimeOffset dateOfBirth, Guid accountId, Guid officeId)
    {
        var user = new Receptionist
        {
            Email = email,
            UserName = email,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            DateOfBirth = dateOfBirth,
            AccountId = accountId,
            OfficeId = officeId
        };

        var result = await _receptionistManager.CreateAsync(user, password);
        ErrorCaster.CheckForUserRegistrationException(result);


    }

    public async Task LoginPatientAsync(string email, string password)
    {
        var user = await _patientManager.FindByIdAsync(email);
        if (user == null)
        {
            throw new InvalidLoginException();
        }

        var passwordHasher = new PasswordHasher<IdentityUser<Guid>>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        ErrorCaster.CheckForInvalidLoginException(result);
    }

    public async Task LoginDoctorAsync(string email, string password)
    {
        var user = await _doctorManager.FindByIdAsync(email);
        if (user == null)
        {
            throw new InvalidLoginException();
        }

        var passwordHasher = new PasswordHasher<IdentityUser<Guid>>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        ErrorCaster.CheckForInvalidLoginException(result);
    }

    public async Task LoginReceptionistAsync(string email, string password)
    {
        var user = await _receptionistManager.FindByIdAsync(email);
        if (user == null)
        {
            throw new UserNotFoundException(email);
        }
        var passwordHasher = new PasswordHasher<IdentityUser<Guid>>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        ErrorCaster.CheckForInvalidLoginException(result);

    }
    public async Task LogoutAsync(string refreshToken)
    {
        var result = await _refreshTokenGenerator.ValidateRefreshTokenAsync(refreshToken);

        if (!result)
        {
            throw new InvalidRefreshTokenException(refreshToken);
        }

        await _refreshTokenGenerator.RevokeRefreshTokenAsync(refreshToken);
    }


    public async Task DeleteDoctorProfileAsync(Guid userId)
    {
        var user = await _doctorManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }
        var result = await _doctorManager.DeleteAsync(user);
        ErrorCaster.CheckForUserNotFoundException(result, userId);

    }

    public async Task DeletePatientProfileAsync(Guid userId)
    {
        var user = await _patientManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }
        var result = await _patientManager.DeleteAsync(user);
        ErrorCaster.CheckForUserNotFoundException(result, userId);
    }

    public async Task DeleteReceptionistProfileAsync(Guid userId)
    {
        var user = await _receptionistManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }
        var result = await _receptionistManager.DeleteAsync(user);
        ErrorCaster.CheckForUserNotFoundException(result, userId);
    }

    public async Task<string> RefreshAccessTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
