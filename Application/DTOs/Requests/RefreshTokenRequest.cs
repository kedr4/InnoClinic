namespace Application.DTOs.Requests;
public record RefreshTokenRequest
    (
    Guid UserId,
    string RefreshToken
    );
