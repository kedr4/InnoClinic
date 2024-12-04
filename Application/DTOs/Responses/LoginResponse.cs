namespace Application.DTOs.Responses;

public record LoginResponse
(
    Guid UserId ,
    string JwtToken ,
    string RefreshToken 
);