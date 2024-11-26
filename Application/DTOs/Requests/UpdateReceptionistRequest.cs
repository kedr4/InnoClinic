using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests;

public class UpdateReceptionistRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public Guid AccountId { get; init; }
    public Guid OfficeId { get; init; }

    public UpdateReceptionistRequest(string firstName, string lastName, string middleName, DateOnly dateOfBirth, Guid accountId, Guid officeId)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        AccountId = accountId;
        OfficeId = officeId;
    }
}
