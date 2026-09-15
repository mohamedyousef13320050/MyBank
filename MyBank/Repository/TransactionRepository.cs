using BankSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankContext context;

        public TransactionRepository(BankContext context)
        {
            this.context = context;
        }

        public List<Transaction> GetByAccountId(int accountId)
        {
            return context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .ToList();
        }

        public void Add(Transaction transaction)
        {
            context.Transactions.Add(transaction);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
