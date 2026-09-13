using BankSystem.BL;
using BankSystem.Models;
using BankSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class BankAccountController : Controller
    {
        private readonly IAccountBL accountBL;
        private readonly ICustomerBL customerBL;

        public BankAccountController(IAccountBL accountBL, ICustomerBL customerBL)
        {
            this.accountBL = accountBL;
            this.customerBL = customerBL;
        }

        public IActionResult Index()
        {
            var accounts = accountBL.GetAllAccounts();
            return View(accounts);
        }

        public IActionResult Create()
        {
            ViewBag.Customers = customerBL.GetAllCustomers().Where(c => c.Status).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AccountCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var (success, errors) = accountBL.CreateAccount(model);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }
            }
            ViewBag.Customers = customerBL.GetAllCustomers().Where(c => c.Status).ToList();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var account = accountBL.GetAccountDetails(id);
            if (account == null) return NotFound();
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatus(int id, AccountStatus status)
        {
            accountBL.ChangeAccountStatus(id, status);
            return RedirectToAction(nameof(Index));
        }
    }
}
