using System.ComponentModel.DataAnnotations;

namespace BankSystem.ViewModel
{
    public class CustomerListVM
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "National ID")]
        public string NationalId { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public bool Status { get; set; }
    }
}
