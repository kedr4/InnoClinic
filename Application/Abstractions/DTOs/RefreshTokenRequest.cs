namespace Application.Abstractions.DTOs;

public record RefreshTokenRequest(string refreshToken, Guid userId);