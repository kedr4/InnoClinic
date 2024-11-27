namespace Application.Exceptions;
public class UserRegistrationException : AppException
{
    public UserRegistrationException(string errors)
        : base($"User registration failed: {errors}", 400)
    {
    }
}