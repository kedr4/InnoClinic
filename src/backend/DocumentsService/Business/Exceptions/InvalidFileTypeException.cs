namespace Business.Exceptions;

public class InvalidFileTypeException() : AppException("File type should be .jpeg or .png", 400)
{
}
