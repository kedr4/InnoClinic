using Domain.Models;

namespace Application.Abstractions.DTOs;

public class LoginUserResponse
{
    public User User { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public LoginUserResponse(User user, string accessToken, string refreshToken)
    {
        User = user;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}