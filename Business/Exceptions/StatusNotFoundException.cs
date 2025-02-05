namespace Business.Exceptions;

public class StatusNotFountException : AppException
{
    public StatusNotFountException(Guid id) : base($"Status with ID {id} not found", 404) { }
}
