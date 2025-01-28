namespace Business.Exceptions;

public class FileAlreadyDeletedException(Guid id) : AppException($"File with id {id} already deleted", 404)
{
}
