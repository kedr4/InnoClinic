namespace Application.Exceptions;

public class EmailIsNotConfirmedException : AppException
{
    public EmailIsNotConfirmedException() : base("Email is not confirmed.", 401)
    { }
}
