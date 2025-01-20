namespace Business.Exceptions;

public class OfficeNotFoundException : AppException
{
    public OfficeNotFoundException(Guid id) : base($"Office with ID {id} not found", 404) { }
}
