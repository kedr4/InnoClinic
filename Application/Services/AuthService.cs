using Application.Abstrsctions.Services;
using Application.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly SignInManager<IdentityUser<Guid>> _signInManager;

    public AuthService(UserManager<IdentityUser<Guid>> UserManager, SignInManager<IdentityUser<Guid>> SignInManager)
    {
        _userManager = UserManager;
        _signInManager = SignInManager;
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

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = ErrorCaster.GetErrorMessages(result);
            throw new UserRegistrationException(errors);
        }
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

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = ErrorCaster.GetErrorMessages(result);
            throw new UserRegistrationException(errors);
        }
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

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = ErrorCaster.GetErrorMessages(result);
            throw new UserRegistrationException(errors);
        }

    }


    public async Task LoginAsync(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new InvalidLoginException();
        }
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task DeleteProfileAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = ErrorCaster.GetErrorMessages(result);
            throw new Exception(errors);
        }
    }

    public async Task<string> RefreshAccessTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }
}
