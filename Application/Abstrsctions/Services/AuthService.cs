using Application.Abstrsctions.Persistance.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstrsctions.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly SignInManager<IdentityUser<Guid>> _signInManager;
    private readonly IDoctorsRepository _doctorsRepository;

    public AuthService(UserManager<IdentityUser<Guid>> userManager, SignInManager<IdentityUser<Guid>> signInManager, IDoctorsRepository doctorsRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _doctorsRepository = doctorsRepository;
    }

    public async Task RegisterPatientAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = new Patient { Email = email, UserName = email };
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task RegisterDoctorAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = new Doctor { Email = email, UserName = email };
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task RegisterReceptionistAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = new Receptionist { Email = email, UserName = email };
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
        if (!result.Succeeded)
            throw new Exception("Invalid login attempt.");
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        await _signInManager.SignOutAsync();
    }

    public async Task DeleteProfileAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) throw new Exception("User not found.");
        await _userManager.DeleteAsync(user);
    }

    public async Task<string> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        // Добавьте логику обновления токена
        throw new NotImplementedException();
    }
}