using BankSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BankContext context;

        public CustomerRepository(BankContext context)
        {
            this.context = context;
        }

        public List<Customer> GetAll()
        {
            return context.Customers
                .Include(c => c.ApplicationUser)
                .ToList();
        }

        public Customer? GetById(int id)
        {
            return context.Customers
                .Include(c => c.ApplicationUser)
                .FirstOrDefault(c => c.Id == id);
        }

        public Customer? GetByUserId(string userId)
        {
            return context.Customers
                .Include(c => c.ApplicationUser)
                .FirstOrDefault(c => c.UserId == userId);
        }

        public Customer? GetByNationalId(string nationalId)
        {
            return context.Customers
                .Include(c => c.ApplicationUser)
                .FirstOrDefault(c => c.NationalId == nationalId);
        }

        public void Add(Customer customer)
        {
            context.Customers.Add(customer);
        }

        public void Update(Customer customer)
        {
            context.Customers.Update(customer);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
