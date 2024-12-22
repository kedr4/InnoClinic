using Application.Abstractions.Services.Auth;

namespace Presentation;

public static class RoleSetup
{
    public static async Task SetupRolesAsync(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var roleService = scope.ServiceProvider.GetRequiredService<IRolesService>();
            var cancellationToken = new CancellationToken();
            await roleService.SetRolesAsync(cancellationToken);
        }
    }
}
