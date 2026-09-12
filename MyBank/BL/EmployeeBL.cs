using BankSystem.Models;
using BankSystem.Repository;
using BankSystem.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace BankSystem.BL
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public EmployeeBL(IEmployeeRepository employeeRepository, UserManager<ApplicationUser> userManager)
        {
            this.employeeRepository = employeeRepository;
            this.userManager = userManager;
        }

        public List<EmployeeListVM> GetAllEmployees()
        {
            var employees = employeeRepository.GetAll();

            return employees.Select(e => new EmployeeListVM
            {
                Id = e.Id,
                FullName = e.FullName,
                EmployeeCode = e.EmployeeCode,
                Position = e.Position,
                Status = e.Status
            }).ToList();
        }

        public EmployeeDetailsVM? GetEmployeeById(int id)
        {
            var employee = employeeRepository.GetById(id);

            if (employee == null)
                return null;

            return new EmployeeDetailsVM
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Email = employee.ApplicationUser?.Email ?? "",
                EmployeeCode = employee.EmployeeCode,
                Position = employee.Position,
                HireDate = employee.HireDate,
                Status = employee.Status
            };
        }

        public async Task<(bool Success, List<string> Errors)> CreateEmployee(EmployeeCreateVM model)
        {
            var errors = new List<string>();

            // Create the ApplicationUser account
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            // Assign Employee role
            await userManager.AddToRoleAsync(user, "Employee");

            // Create Employee record
            var employee = new Employee
            {
                UserId = user.Id,
                FullName = model.FullName,
                EmployeeCode = model.EmployeeCode,
                Position = model.Position,
                HireDate = model.HireDate,
                Status = true
            };

            employeeRepository.Add(employee);
            employeeRepository.Save();

            return (true, errors);
        }

        public async Task<(bool Success, List<string> Errors)> UpdateEmployee(EmployeeEditVM model)
        {
            var errors = new List<string>();

            var employee = employeeRepository.GetById(model.Id);

            if (employee == null)
            {
                errors.Add("Employee not found.");
                return (false, errors);
            }

            employee.FullName = model.FullName;
            employee.EmployeeCode = model.EmployeeCode;
            employee.Position = model.Position;
            employee.HireDate = model.HireDate;

            // Update the ApplicationUser FullName as well
            var user = await userManager.FindByIdAsync(employee.UserId);
            if (user != null)
            {
                user.FullName = model.FullName;
                await userManager.UpdateAsync(user);
            }

            employeeRepository.Update(employee);
            employeeRepository.Save();

            return (true, errors);
        }

        public bool ActivateEmployee(int id)
        {
            var employee = employeeRepository.GetById(id);

            if (employee == null)
                return false;

            employee.Status = true;
            employeeRepository.Update(employee);
            employeeRepository.Save();

            return true;
        }

        public bool DeactivateEmployee(int id)
        {
            var employee = employeeRepository.GetById(id);

            if (employee == null)
                return false;

            employee.Status = false;
            employeeRepository.Update(employee);
            employeeRepository.Save();

            return true;
        }
    }
}
