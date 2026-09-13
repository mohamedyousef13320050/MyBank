using BankSystem.ViewModel;

namespace BankSystem.BL
{
    public interface ICustomerBL
    {
        // Employee Operations
        List<CustomerListVM> GetAllCustomers();
        CustomerDetailsVM? GetCustomerById(int id);
        Task<(bool Success, List<string> Errors)> CreateCustomer(CustomerCreateVM model);
        Task<(bool Success, List<string> Errors)> UpdateCustomer(CustomerEditVM model);
        bool ActivateCustomer(int id);
        bool DeactivateCustomer(int id);

        // Customer Operations
        CustomerProfileVM? GetMyProfile(string userId);
        Task<(bool Success, List<string> Errors)> UpdateMyProfile(string userId, CustomerProfileVM model);
    }
}
