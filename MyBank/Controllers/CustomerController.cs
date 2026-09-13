using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankSystem.BL;
using BankSystem.ViewModel;

namespace BankSystem.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class CustomerController : Controller
    {
        private readonly ICustomerBL customerBL;

        public CustomerController(ICustomerBL customerBL)
        {
            this.customerBL = customerBL;
        }

        public IActionResult Index(string? searchString)
        {
            var customers = customerBL.GetAllCustomers();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var trimmed = searchString.Trim();
                customers = customers
                    .Where(c => (!string.IsNullOrEmpty(c.FullName) && c.FullName.Contains(trimmed, StringComparison.OrdinalIgnoreCase))
                             || (!string.IsNullOrEmpty(c.NationalId) && c.NationalId.Contains(trimmed, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }

            ViewBag.SearchString = searchString;
            ViewData["CurrentFilter"] = searchString;

            return View(customers);
        }

        public IActionResult Details(int id)
        {
            var customer = customerBL.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var (success, errors) = await customerBL.CreateCustomer(model);

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
            var customer = customerBL.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            var model = new CustomerEditVM
            {
                Id = customer.Id,
                FullName = customer.FullName,
                NationalId = customer.NationalId,
                DateOfBirth = customer.DateOfBirth,
                Address = customer.Address,
                PhoneNumber = customer.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerEditVM model)
        {
            if (ModelState.IsValid)
            {
                var (success, errors) = await customerBL.UpdateCustomer(model);

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
            customerBL.ActivateCustomer(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(int id)
        {
            customerBL.DeactivateCustomer(id);
            return RedirectToAction("Index");
        }
    }
}
