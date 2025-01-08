namespace Application.Abstractions.DTOs;

public record RefreshTokenRequest(string RefreshToken, Guid UserId);