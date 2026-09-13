using System.Security.Claims;
using BankSystem.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerAccountController : Controller
    {
        private readonly IAccountBL accountBL;

        public CustomerAccountController(IAccountBL accountBL)
        {
            this.accountBL = accountBL;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var accounts = accountBL.GetMyAccounts(userId);
            return View(accounts);
        }

        public IActionResult Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var account = accountBL.GetMyAccountDetails(userId, id);
            if (account == null) return NotFound();

            return View(account);
        }
    }
}
