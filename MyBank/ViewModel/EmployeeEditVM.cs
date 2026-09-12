using System.ComponentModel.DataAnnotations;

namespace BankSystem.ViewModel
{
    public class EmployeeEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employee Code is required")]
        [MaxLength(20, ErrorMessage = "Employee Code cannot exceed 20 characters")]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required")]
        [MaxLength(50, ErrorMessage = "Position cannot exceed 50 characters")]
        [Display(Name = "Position")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hire Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public bool Status { get; set; }
    }
}
