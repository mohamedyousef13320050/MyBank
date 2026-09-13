using System.Security.Claims;
using BankSystem.BL;
using BankSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerTransactionController : Controller
    {
        private readonly ITransactionBL transactionBL;
        private readonly IAccountBL accountBL;

        public CustomerTransactionController(ITransactionBL transactionBL, IAccountBL accountBL)
        {
            this.transactionBL = transactionBL;
            this.accountBL = accountBL;
        }

        public IActionResult History(int accountId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var history = transactionBL.GetCustomerTransactions(userId, accountId);
            if (history == null) return NotFound();

            return View(history);
        }

        public IActionResult Transfer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var myAccounts = accountBL.GetMyAccounts(userId).Where(a => a.Status == BankSystem.Models.AccountStatus.Active).ToList();
            ViewBag.MyAccounts = myAccounts;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transfer(TransferVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            if (ModelState.IsValid)
            {
                var (success, message) = transactionBL.ProcessTransfer(userId, model);
                if (success)
                {
                    TempData["Success"] = message;
                    return RedirectToAction("Index", "CustomerAccount");
                }
                ModelState.AddModelError("", message);
            }

            var myAccounts = accountBL.GetMyAccounts(userId).Where(a => a.Status == BankSystem.Models.AccountStatus.Active).ToList();
            ViewBag.MyAccounts = myAccounts;
            return View(model);
        }
    }
}
