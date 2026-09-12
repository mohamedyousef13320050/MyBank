namespace BankSystem.ViewModel
{
    public class AdminReportVM
    {
        public int TotalCustomers { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalBranches { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWithdrawals { get; set; }
        public decimal TotalTransfers { get; set; }
    }
}
