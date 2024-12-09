using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Receptionist : ApplicationUser
{
    public Guid OfficeId { get; set; }
}
