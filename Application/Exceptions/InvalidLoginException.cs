namespace Application.Exceptions;
public class InvalidLoginException : AppException
{
    public InvalidLoginException()
        : base("Invalid login attempt.", 401)
    { }
}

