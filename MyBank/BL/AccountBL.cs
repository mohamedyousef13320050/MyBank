using BankSystem.Models;
using BankSystem.Repository;
using BankSystem.ViewModel;

namespace BankSystem.BL
{
    public class AccountBL : IAccountBL
    {
        private readonly IAccountRepository accountRepository;
        private readonly ICustomerRepository customerRepository;

        public AccountBL(IAccountRepository accountRepository, ICustomerRepository customerRepository)
        {
            this.accountRepository = accountRepository;
            this.customerRepository = customerRepository;
        }

        public List<AccountListVM> GetAllAccounts()
        {
            return accountRepository.GetAll().Select(a => new AccountListVM
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                CustomerName = a.Customer.FullName,
                Balance = a.Balance,
                Status = a.Status
            }).ToList();
        }

        public AccountDetailsVM? GetAccountDetails(int id)
        {
            var account = accountRepository.GetById(id);
            if (account == null) return null;

            return new AccountDetailsVM
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                CustomerName = account.Customer.FullName,
                Balance = account.Balance,
                CreatedAt = account.CreatedAt,
                Status = account.Status
            };
        }

        public (bool Success, List<string> Errors) CreateAccount(AccountCreateVM model)
        {
            var errors = new List<string>();

            // Validate Customer exists
            var customer = customerRepository.GetById(model.CustomerId);
            if (customer == null)
            {
                errors.Add("Selected customer does not exist.");
                return (false, errors);
            }

            // Generate unique 10-digit account number (simplified generation)
            string accountNum;
            do
            {
                accountNum = new Random().Next(1000000000, int.MaxValue).ToString();
            } while (accountRepository.GetByAccountNumber(accountNum) != null);

            var account = new Account
            {
                AccountNumber = accountNum,
                Balance = model.InitialBalance, // DB constraint ensures >= 0
                CustomerId = model.CustomerId,
                CreatedAt = DateTime.Now,
                Status = AccountStatus.Active
            };

            accountRepository.Add(account);
            accountRepository.Save();

            return (true, errors);
        }

        public bool ChangeAccountStatus(int id, AccountStatus newStatus)
        {
            var account = accountRepository.GetById(id);
            if (account == null) return false;

            account.Status = newStatus;
            accountRepository.Update(account);
            accountRepository.Save();
            return true;
        }

        public List<AccountListVM> GetMyAccounts(string userId)
        {
            var customer = customerRepository.GetByUserId(userId);
            if (customer == null) return new List<AccountListVM>();

            var accounts = accountRepository.GetByCustomerId(customer.Id);
            return accounts.Select(a => new AccountListVM
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                CustomerName = a.Customer.FullName,
                Balance = a.Balance,
                Status = a.Status
            }).ToList();
        }

        public AccountDetailsVM? GetMyAccountDetails(string userId, int accountId)
        {
            var customer = customerRepository.GetByUserId(userId);
            if (customer == null) return null;

            var account = accountRepository.GetById(accountId);
            // Server-side ownership validation:
            if (account == null || account.CustomerId != customer.Id) return null;

            return new AccountDetailsVM
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                CustomerName = account.Customer.FullName,
                Balance = account.Balance,
                CreatedAt = account.CreatedAt,
                Status = account.Status
            };
        }
    }
}
