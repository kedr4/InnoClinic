using Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
namespace Application.Helpers;
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

    public static void CheckForUserNotFoundException([DoesNotReturnIf(true)] IdentityResult result, Guid id)
    {
        if (!result.Succeeded)
        {
            throw new UserNotFoundException(id);
        }
    }

    public static void CheckForInvalidLoginException([DoesNotReturnIf(true)] Microsoft.AspNetCore.Identity.PasswordVerificationResult result)
    {
        if (result == 0)
        {
            throw new InvalidLoginException();
        }
    }

    public static void CheckForUserRegistrationException([DoesNotReturnIf(true)] IdentityResult result)
    {
        if (!result.Succeeded)
        {
            var errors = GetErrorMessages(result);
            throw new UserRegistrationException(errors);
        }
    }

}
