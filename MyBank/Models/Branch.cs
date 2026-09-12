using System.ComponentModel.DataAnnotations;

namespace BankSystem.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

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

        // Navigation Property
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
