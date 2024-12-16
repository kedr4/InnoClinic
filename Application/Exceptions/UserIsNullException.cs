namespace Application.Exceptions;

public class UserIsNullException : AppException
{
    public UserIsNullException() : base("User is null.", 404)
    { }
}
