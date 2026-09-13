using BankSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankContext context;

        public AccountRepository(BankContext context)
        {
            this.context = context;
        }

        public List<Account> GetAll()
        {
            return context.Accounts
                .Include(a => a.Customer)
                .ToList();
        }

        public List<Account> GetByCustomerId(int customerId)
        {
            return context.Accounts
                .Include(a => a.Customer)
                .Where(a => a.CustomerId == customerId)
                .ToList();
        }

        public Account? GetById(int id)
        {
            return context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefault(a => a.Id == id);
        }

        public Account? GetByAccountNumber(string accountNumber)
        {
            return context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefault(a => a.AccountNumber == accountNumber);
        }

        public void Add(Account account)
        {
            context.Accounts.Add(account);
        }

        public void Update(Account account)
        {
            context.Accounts.Update(account);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
