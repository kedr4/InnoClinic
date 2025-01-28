namespace Business.Exceptions;

public class FileDataNotFoundException(Guid id) : AppException($"File with id {id} not found.", 404)
{ }
