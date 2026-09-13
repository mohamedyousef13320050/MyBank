using BankSystem.Models;

namespace BankSystem.Repository
{
    public interface IAccountRepository
    {
        List<Account> GetAll();
        List<Account> GetByCustomerId(int customerId);
        Account? GetById(int id);
        Account? GetByAccountNumber(string accountNumber);
        void Add(Account account);
        void Update(Account account);
        void Save();
    }
}
