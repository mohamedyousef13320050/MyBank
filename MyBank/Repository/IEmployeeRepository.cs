using BankSystem.Models;

namespace BankSystem.Repository
{
    public interface IEmployeeRepository
    {
        List<Employee> GetAll();

        Employee? GetById(int id);

        Employee? GetByUserId(string userId);

        void Add(Employee employee);

        void Update(Employee employee);

        void Save();
    }
}
