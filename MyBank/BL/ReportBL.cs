using BankSystem.Models;
using BankSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.BL
{
    public class ReportBL : IReportBL
    {
        private readonly BankContext context;

        public ReportBL(BankContext context)
        {
            this.context = context;
        }

        public AdminReportVM GetDashboardReport()
        {
            var vm = new AdminReportVM();

            // Aggregations using Entity Framework
            vm.TotalCustomers    = context.Customers.Count();
            vm.TotalEmployees    = context.Employees.Count();
            vm.TotalAccounts     = context.Accounts.Count();
            vm.TotalTransactions = context.Transactions.Count();
            vm.TotalBranches     = context.Branches.Count();

            // Handle potential null/empty tables for sums
            if (vm.TotalAccounts > 0)
            {
                vm.TotalBalance = context.Accounts.Sum(a => (decimal?)a.Balance) ?? 0;
            }

            if (vm.TotalTransactions > 0)
            {
                vm.TotalDeposits = context.Transactions
                    .Where(t => t.Type == TransactionType.Deposit)
                    .Sum(t => (decimal?)t.Amount) ?? 0;

                vm.TotalWithdrawals = context.Transactions
                    .Where(t => t.Type == TransactionType.Withdraw)
                    .Sum(t => (decimal?)t.Amount) ?? 0;

                // Transfer creates two transactions (one debit, one credit).
                // Dividing by 2 gets the real transferred volume.
                var totalTransferTransactionsAmount = context.Transactions
                    .Where(t => t.Type == TransactionType.Transfer)
                    .Sum(t => (decimal?)t.Amount) ?? 0;
                
                vm.TotalTransfers = totalTransferTransactionsAmount / 2;
            }

            return vm;
        }
    }
}
