using BankSystem.Models;
using BankSystem.Repository;
using BankSystem.ViewModel;

namespace BankSystem.BL
{
    public class TransactionBL : ITransactionBL
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly BankContext context;

        public TransactionBL(ITransactionRepository transactionRepository, IAccountRepository accountRepository, ICustomerRepository customerRepository, BankContext context)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = accountRepository;
            this.customerRepository = customerRepository;
            this.context = context;
        }

        public (bool Success, string Message) ProcessDeposit(DepositVM model)
        {
            var account = accountRepository.GetByAccountNumber(model.AccountNumber);
            if (account == null)
                return (false, "Account not found.");

            if (account.Status != AccountStatus.Active)
                return (false, $"Account is {account.Status} and cannot receive deposits.");

            if (model.Amount <= 0)
                return (false, "Amount must be greater than zero.");

            // Update balance
            account.Balance += model.Amount;

            // Create transaction record
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = model.Amount,
                Type = TransactionType.Deposit,
                Date = DateTime.Now,
                Description = model.Description
            };

            transactionRepository.Add(transaction);
            accountRepository.Update(account);
            
            // Single SaveChanges guarantees atomicity (transactional behavior)
            accountRepository.Save();

            return (true, "Deposit successful.");
        }

        public (bool Success, string Message) ProcessWithdraw(WithdrawVM model)
        {
            var account = accountRepository.GetByAccountNumber(model.AccountNumber);
            if (account == null)
                return (false, "Account not found.");

            if (account.Status != AccountStatus.Active)
                return (false, $"Account is {account.Status} and cannot process withdrawals.");

            if (model.Amount <= 0)
                return (false, "Amount must be greater than zero.");

            if (account.Balance < model.Amount)
                return (false, "Insufficient balance.");

            // Update balance
            account.Balance -= model.Amount;

            // Create transaction record
            var transaction = new Transaction
            {
                AccountId = account.Id,
                Amount = model.Amount,
                Type = TransactionType.Withdraw,
                Date = DateTime.Now,
                Description = model.Description
            };

            transactionRepository.Add(transaction);
            accountRepository.Update(account);
            
            // Single SaveChanges guarantees atomicity
            accountRepository.Save();

            return (true, "Withdrawal successful.");
        }

        public TransactionHistoryVM? GetCustomerTransactions(string userId, int accountId)
        {
            var customer = customerRepository.GetByUserId(userId);
            if (customer == null) return null;

            var account = accountRepository.GetById(accountId);
            // Server-side ownership validation
            if (account == null || account.CustomerId != customer.Id) return null;

            var transactions = transactionRepository.GetByAccountId(accountId);

            var vm = new TransactionHistoryVM
            {
                AccountId = account.Id,
                AccountNumber = account.AccountNumber,
                CurrentBalance = account.Balance,
                Transactions = transactions.Select(t => new TransactionItemVM
                {
                    Date = t.Date,
                    Type = t.Type,
                    Amount = t.Amount,
                    Description = t.Description
                }).ToList()
            };

            return vm;
        }

        public (bool Success, string Message) ProcessTransfer(string userId, TransferVM model)
        {
            var customer = customerRepository.GetByUserId(userId);
            if (customer == null) return (false, "Customer not found.");

            var sender = accountRepository.GetById(model.FromAccountId);
            if (sender == null || sender.CustomerId != customer.Id)
                return (false, "Invalid sender account.");

            var receiver = accountRepository.GetByAccountNumber(model.ToAccountNumber);
            if (receiver == null)
                return (false, "Receiver account not found.");

            if (sender.Id == receiver.Id)
                return (false, "Cannot transfer to the same account.");

            if (sender.Status != AccountStatus.Active)
                return (false, "Sender account is not active.");

            if (receiver.Status != AccountStatus.Active)
                return (false, "Receiver account is not active.");

            if (model.Amount <= 0)
                return (false, "Transfer amount must be greater than zero.");

            if (sender.Balance < model.Amount)
                return (false, "Insufficient balance.");

            using var transaction = context.Database.BeginTransaction();
            try
            {
                // 1. Debit Sender
                sender.Balance -= model.Amount;
                accountRepository.Update(sender);

                var senderTransaction = new Transaction
                {
                    AccountId = sender.Id,
                    Amount = model.Amount,
                    Type = TransactionType.Transfer,
                    Date = DateTime.Now,
                    Description = $"Transfer to {receiver.AccountNumber}: {model.Description}"
                };
                transactionRepository.Add(senderTransaction);

                // 2. Credit Receiver
                receiver.Balance += model.Amount;
                accountRepository.Update(receiver);

                var receiverTransaction = new Transaction
                {
                    AccountId = receiver.Id,
                    Amount = model.Amount,
                    Type = TransactionType.Transfer,
                    Date = DateTime.Now,
                    Description = $"Transfer from {sender.AccountNumber}: {model.Description}"
                };
                transactionRepository.Add(receiverTransaction);

                // 3. Save all changes
                accountRepository.Save();

                // 4. Commit DB Transaction
                transaction.Commit();
                return (true, "Transfer successful.");
            }
            catch (Exception)
            {
                transaction.Rollback();
                return (false, "An error occurred during transfer. Transaction rolled back.");
            }
        }
    }
}
