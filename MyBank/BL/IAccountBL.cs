using BankSystem.ViewModel;

namespace BankSystem.BL
{
    public interface IAccountBL
    {
        // Employee Operations
        List<AccountListVM> GetAllAccounts();
        AccountDetailsVM? GetAccountDetails(int id);
        (bool Success, List<string> Errors) CreateAccount(AccountCreateVM model);
        bool ChangeAccountStatus(int id, BankSystem.Models.AccountStatus newStatus);

        // Customer Operations
        List<AccountListVM> GetMyAccounts(string userId);
        AccountDetailsVM? GetMyAccountDetails(string userId, int accountId);
    }
}
