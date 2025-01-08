namespace Application.Exceptions;

public class InvalidConfirmTokenException : AppException
{
    public InvalidConfirmTokenException()
        : base("Invalid confirm token.", 401)
    { }
}

