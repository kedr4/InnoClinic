namespace Application.DTOs.Responses;

public record LoginResponse
(
    Guid UserId = default,
    string JwtToken = default,
    string RefreshToken = default
);