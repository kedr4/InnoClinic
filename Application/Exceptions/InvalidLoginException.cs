namespace Application.Exceptions;
public class InvalidLoginException : Exception
{
    public InvalidLoginException()
        : base("Invalid login attempt.")
    {
    }
}

