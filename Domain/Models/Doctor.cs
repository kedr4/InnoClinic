using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Doctor : ApplicationUser
{
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    public int CareerStartYear { get; set; }
}