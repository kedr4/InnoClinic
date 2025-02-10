namespace Business.Exceptions;

public class NotAReceptionistException : AppException
{
    public NotAReceptionistException(Guid id) : base($"User with id {id} is not a receptionist. ", 403)
    {

    }
}
