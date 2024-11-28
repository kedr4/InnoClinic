namespace Application.Exceptions;
public class InvalidRefreshTokenException : AppException
{
    public InvalidRefreshTokenException(string refreshToken)
       : base($"Refresh token is not valid {refreshToken}", 401)
    { }

}
