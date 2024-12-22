namespace Application.Abstractions.DTOs;

public record ConfirmMailRequest(Guid UserId, string Token);
