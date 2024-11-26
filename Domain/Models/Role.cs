using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class Role : IdentityUserRole<Guid>
    {
        public string Name { get; set; }
    }

}