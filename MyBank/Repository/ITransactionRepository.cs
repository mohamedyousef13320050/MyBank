using BankSystem.Models;

namespace BankSystem.Repository
{
    public interface ITransactionRepository
    {
        List<Transaction> GetByAccountId(int accountId);
        void Add(Transaction transaction);
        void Save();
    }
}
