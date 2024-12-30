namespace Application.Exceptions;

public class InvalidRefreshTokenException : AppException
{
    public InvalidRefreshTokenException(string refreshToken)
       : base($"Invalid refresh token: {refreshToken}", 401)
    { }

}
