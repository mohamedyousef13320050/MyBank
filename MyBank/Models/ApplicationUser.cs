using Microsoft.AspNetCore.Identity;

namespace BankSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
