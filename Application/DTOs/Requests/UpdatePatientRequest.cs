namespace Application.DTOs.Requests;

public record UpdatePatientRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string MiddleName { get; init; } = string.Empty;
    public bool IsLinkedToAccount { get; init; }
    public DateTimeOffset DateOfBirth { get; init; }

    public UpdatePatientRequest(string firstName, string lastName, string middleName, bool isLinkedToAccount, DateTimeOffset dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        IsLinkedToAccount = isLinkedToAccount;
        DateOfBirth = dateOfBirth;
    }
}
