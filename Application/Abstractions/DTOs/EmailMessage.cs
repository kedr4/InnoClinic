namespace Application.Abstractions.DTOs;

public record EmailMessage(
    string Addressee,
    string Subject,
    string Content,
    string AddresseeName
);