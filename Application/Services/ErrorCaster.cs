using Microsoft.AspNetCore.Identity;
namespace Application.Services;
public static class ErrorCaster
{
    public static string GetErrorMessages(IdentityResult result)
    {

        if (!result.Succeeded)
        {
            return string.Join(", ", result.Errors.Select(e => e.Description));
        }
        return string.Empty;
    }

}
