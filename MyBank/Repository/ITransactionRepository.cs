using BankSystem.Models;

namespace BankSystem.Repository
{
    public interface ITransactionRepository
    {
        List<Transaction> GetByAccountId(int accountId);
        Transaction? GetById(int id);
        void Add(Transaction transaction);
        void Save();
    }
}
