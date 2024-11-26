using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests;

public class UpdatePatientRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public bool IsLinkedToAccount { get; init; }
    public DateOnly DateOfBirth { get; init; }

    public UpdatePatientRequest(string firstName, string lastName, string middleName, bool isLinkedToAccount, DateOnly dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        IsLinkedToAccount = isLinkedToAccount;
        DateOfBirth = dateOfBirth;
    }
}
