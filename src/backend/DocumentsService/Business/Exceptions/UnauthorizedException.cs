namespace Business.Exceptions;

public class UnauthorizedException() : AppException("Unauthorized access denied", 401)
{
}
