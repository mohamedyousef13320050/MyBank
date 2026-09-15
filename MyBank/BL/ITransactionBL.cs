using BankSystem.ViewModel;

namespace BankSystem.BL
{
    public interface ITransactionBL
    {
        (bool Success, string Message) ProcessDeposit(DepositVM model);
        (bool Success, string Message) ProcessWithdraw(WithdrawVM model);
        TransactionHistoryVM? GetCustomerTransactions(string userId, int accountId);
        (bool Success, string Message) ProcessTransfer(string userId, TransferVM model);
    }
}
