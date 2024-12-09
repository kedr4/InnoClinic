using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Patient : ApplicationUser
{
    public bool isLinkedToAccount { get; set; }
}
