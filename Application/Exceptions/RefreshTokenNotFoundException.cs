
namespace Application.Exceptions;
public class RefreshTokenNotFoundException : AppException
{
    public RefreshTokenNotFoundException() : base("Refresh token not found.", 404)
    { }
}
