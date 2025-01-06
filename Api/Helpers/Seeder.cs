using Domain.Models;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace Presentation.Helpers;

public static class Seeder
{
    public static async Task SetupRolesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
        var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

        var patientRole = new UserRole()
        {
            Id = Guid.Parse("3D909293-0B9F-4777-974C-DAC10F875A4F"),
            //Id = Guid.NewGuid(),
            Name = nameof(Roles.Patient)
        };

        var doctorRole = new UserRole()
        {
            Id = Guid.Parse("151FDDFE-DFCD-409C-9125-D2AF800A5C7A"),
            //Id = Guid.NewGuid(),
            Name = nameof(Roles.Doctor)
        };

        var receptionistRole = new UserRole()
        {
            Id = Guid.Parse("BBAB4AB6-AF5F-4BA5-9289-6192476CEA65"),
            //Id = Guid.NewGuid(),
            Name = nameof(Roles.Receptionist)
        };

        var isCreated = await dbContext.Database.EnsureCreatedAsync();

        if (isCreated)
        {
            await roleManager.CreateAsync(patientRole);
            await roleManager.CreateAsync(doctorRole);
            await roleManager.CreateAsync(receptionistRole);
        }
    }

    public static async Task SeedReceptionistAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var receptionist = new User()
        {
            Id = Guid.Parse("5E27546F-6B81-4B8D-9F64-02F3284E7E98"),
            Email = "confirmemail@email.com",
            UserName = "receptionist",
            EmailConfirmed = true
        };

        await userManager.AddToRoleAsync(receptionist, nameof(Roles.Receptionist));
    }
}
