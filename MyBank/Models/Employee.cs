using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Position { get; set; } = string.Empty;

        // Nullable because Branch entity is not implemented until Card 3
        public int? BranchId { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public bool Status { get; set; } = true;

        // Navigation Properties
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [ForeignKey("BranchId")]
        public virtual Branch? Branch { get; set; }
    }
}
