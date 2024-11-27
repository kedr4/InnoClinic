namespace Application.Exceptions;
public class UserRegistrationException : Exception
{
    public UserRegistrationException(string errors)
        : base($"User registration failed: {errors}")
    {
    }
}