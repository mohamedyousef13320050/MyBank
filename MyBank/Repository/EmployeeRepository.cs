using BankSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly BankContext context;

        public EmployeeRepository(BankContext context)
        {
            this.context = context;
        }

        public List<Employee> GetAll()
        {
            return context.Employees
                .Include(e => e.ApplicationUser)
                .ToList();
        }

        public Employee? GetById(int id)
        {
            return context.Employees
                .Include(e => e.ApplicationUser)
                .FirstOrDefault(e => e.Id == id);
        }

        public Employee? GetByUserId(string userId)
        {
            return context.Employees
                .Include(e => e.ApplicationUser)
                .FirstOrDefault(e => e.UserId == userId);
        }

        public void Add(Employee employee)
        {
            context.Employees.Add(employee);
        }

        public void Update(Employee employee)
        {
            context.Employees.Update(employee);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
