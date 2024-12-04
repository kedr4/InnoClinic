namespace Application.Exceptions;

public class UserIdEmptyException : AppException
{
    public UserIdEmptyException() : base("User id is empty.", 400)
    { }
}
