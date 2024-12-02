namespace Application.DTOs.Requests;

public record DeleteProfileRequest
(

    Guid UserId,
    string Password,
    RolesEnum Role
);