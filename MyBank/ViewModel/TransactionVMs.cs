using System.ComponentModel.DataAnnotations;
using BankSystem.Models;

namespace BankSystem.ViewModel
{
    public class DepositVM
    {
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Deposit amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = "Deposit";
    }

    public class WithdrawVM
    {
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Withdrawal amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = "Withdrawal";
    }

    public class TransactionHistoryVM
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public List<TransactionItemVM> Transactions { get; set; } = new List<TransactionItemVM>();
    }

    public class TransactionItemVM
    {
        public DateTime Date { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class TransferVM
    {
        [Required]
        [Display(Name = "From Account")]
        public int FromAccountId { get; set; }

        [Required]
        [Display(Name = "To Account Number")]
        public string ToAccountNumber { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Transfer amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } = "Transfer";
    }
}
