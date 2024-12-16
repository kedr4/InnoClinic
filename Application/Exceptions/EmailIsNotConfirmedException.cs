namespace Application.Exceptions;

public class EmailIsNotConfirmedException : AppException
{
    public EmailIsNotConfirmedException() : base("User id is empty.", 401)
    {
    }
}
