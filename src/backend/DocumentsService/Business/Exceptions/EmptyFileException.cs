namespace Business.Exceptions;

public class EmptyFileException() : AppException("File is empty", 400)
{
}
