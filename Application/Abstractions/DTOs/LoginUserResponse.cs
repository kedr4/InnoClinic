namespace Application.Abstractions.DTOs;

public record LoginUserResponse(Guid UserId, string AccessToken, string RefreshToken);
