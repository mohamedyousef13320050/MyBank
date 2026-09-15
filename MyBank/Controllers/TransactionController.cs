using BankSystem.BL;
using BankSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [Authorize(Roles = "Employee")]
    public class TransactionController : Controller
    {
        private readonly ITransactionBL transactionBL;

        public TransactionController(ITransactionBL transactionBL)
        {
            this.transactionBL = transactionBL;
        }

        public IActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(DepositVM model)
        {
            if (ModelState.IsValid)
            {
                var (success, message) = transactionBL.ProcessDeposit(model);
                if (success)
                {
                    TempData["Success"] = message;
                    return RedirectToAction(nameof(Deposit));
                }
                ModelState.AddModelError("", message);
            }
            return View(model);
        }

        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Withdraw(WithdrawVM model)
        {
            if (ModelState.IsValid)
            {
                var (success, message) = transactionBL.ProcessWithdraw(model);
                if (success)
                {
                    TempData["Success"] = message;
                    return RedirectToAction(nameof(Withdraw));
                }
                ModelState.AddModelError("", message);
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var vm = transactionBL.GetTransactionDetails(id);
            if (vm == null) return NotFound();
            return View(vm);
        }
    }
}
