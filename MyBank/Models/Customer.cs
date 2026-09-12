using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)] // Standard length for National ID in many regions
        public string NationalId { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public bool Status { get; set; } = true;

        // Navigation Properties
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
