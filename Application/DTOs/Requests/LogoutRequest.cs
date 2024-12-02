namespace Application.DTOs.Requests;
public record LogoutRequest
    (
    Guid UserId,
    string RefreshToken
    );
