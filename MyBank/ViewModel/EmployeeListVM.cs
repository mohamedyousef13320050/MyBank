using System.ComponentModel.DataAnnotations;

namespace BankSystem.ViewModel
{
    public class EmployeeListVM
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; } = string.Empty;

        [Display(Name = "Position")]
        public string Position { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
}
