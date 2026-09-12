using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankSystem.BL;
using BankSystem.ViewModel;

namespace BankSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeBL employeeBL;

        public EmployeeController(IEmployeeBL employeeBL)
        {
            this.employeeBL = employeeBL;
        }

        public IActionResult Index()
        {
            var employees = employeeBL.GetAllEmployees();
            return View(employees);
        }

        public IActionResult Details(int id)
        {
            var employee = employeeBL.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var (success, errors) = await employeeBL.CreateEmployee(model);

                if (success)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = employeeBL.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            var model = new EmployeeEditVM
            {
                Id = employee.Id,
                FullName = employee.FullName,
                EmployeeCode = employee.EmployeeCode,
                Position = employee.Position,
                HireDate = employee.HireDate,
                Status = employee.Status
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeEditVM model)
        {
            if (ModelState.IsValid)
            {
                var (success, errors) = await employeeBL.UpdateEmployee(model);

                if (success)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(int id)
        {
            employeeBL.ActivateEmployee(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(int id)
        {
            employeeBL.DeactivateEmployee(id);
            return RedirectToAction("Index");
        }
    }
}
