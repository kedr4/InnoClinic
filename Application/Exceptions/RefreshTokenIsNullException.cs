using Application.Exceptions;

public class RefreshTokenIsNullException : AppException
{
    public RefreshTokenIsNullException()
       : base($"Refresh token is null ", 401)
    { }

}
