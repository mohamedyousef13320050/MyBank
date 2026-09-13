using BankSystem.Models;

namespace BankSystem.Repository
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll();

        Customer? GetById(int id);

        Customer? GetByUserId(string userId);

        Customer? GetByNationalId(string nationalId);

        void Add(Customer customer);

        void Update(Customer customer);

        void Save();
    }
}
