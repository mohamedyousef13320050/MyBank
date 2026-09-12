using System.ComponentModel.DataAnnotations;

namespace BankSystem.ViewModel
{
    public class EmployeeDetailsVM
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; } = string.Empty;

        [Display(Name = "Position")]
        public string Position { get; set; } = string.Empty;

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
}
