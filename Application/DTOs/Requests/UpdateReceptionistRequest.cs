namespace Application.DTOs.Requests;

public record UpdateReceptionistRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string MiddleName { get; init; } = string.Empty;
    public DateTimeOffset DateOfBirth { get; init; }
    public Guid AccountId { get; init; }
    public Guid OfficeId { get; init; }

    public UpdateReceptionistRequest(string firstName, string lastName, string middleName, DateTimeOffset dateOfBirth, Guid accountId, Guid officeId)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        AccountId = accountId;
        OfficeId = officeId;
    }
}
