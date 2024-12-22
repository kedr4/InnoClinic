using Application.Abstractions.Services.Auth;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class RolesService(RoleManager<UserRole> roleManager) : IRolesService
{
    public async Task SetRolesAsync(CancellationToken cancellationToken)
    {
        var patientRole = new UserRole()
        {
            Id = Guid.NewGuid(),
            Name = nameof(Roles.Patient)
        };

        var doctorRole = new UserRole()
        {
            Id = Guid.NewGuid(),
            Name = nameof(Roles.Doctor)
        };

        var receptionistRole = new UserRole()
        {
            Id = Guid.NewGuid(),
            Name = nameof(Roles.Receptionist)
        };

        await roleManager.CreateAsync(patientRole);
        await roleManager.CreateAsync(doctorRole);
        await roleManager.CreateAsync(receptionistRole);
    }
}
