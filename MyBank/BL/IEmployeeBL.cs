using BankSystem.ViewModel;

namespace BankSystem.BL
{
    public interface IEmployeeBL
    {
        List<EmployeeListVM> GetAllEmployees();

        EmployeeDetailsVM? GetEmployeeById(int id);

        Task<(bool Success, List<string> Errors)> CreateEmployee(EmployeeCreateVM model);

        Task<(bool Success, List<string> Errors)> UpdateEmployee(EmployeeEditVM model);

        bool ActivateEmployee(int id);

        bool DeactivateEmployee(int id);
    }
}
