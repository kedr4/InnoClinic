namespace Application.Exceptions;

public class UserAlreadyExistsException : AppException
{
    public UserAlreadyExistsException() : base("User already exists.", 400)
    { }
}
