namespace Application.DTOs.Requests;

public record UpdateReceptionistRequest(

    string FirstName,
    string LastName,
    string MiddleName,
    DateTimeOffset DateOfBirth = default,
    Guid AccountId = default,
    Guid OfficeId = default

    );
