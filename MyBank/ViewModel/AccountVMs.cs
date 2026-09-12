using System.ComponentModel.DataAnnotations;
using BankSystem.Models;

namespace BankSystem.ViewModel
{
    public class AccountListVM
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public AccountStatus Status { get; set; }
    }

    public class AccountCreateVM
    {
        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Initial balance cannot be negative.")]
        [Display(Name = "Initial Balance")]
        public decimal InitialBalance { get; set; }
    }

    public class AccountDetailsVM
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public AccountStatus Status { get; set; }
    }
}
