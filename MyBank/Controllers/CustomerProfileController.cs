using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BankSystem.BL;
using BankSystem.ViewModel;

namespace BankSystem.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerProfileController : Controller
    {
        private readonly ICustomerBL customerBL;

        public CustomerProfileController(ICustomerBL customerBL)
        {
            this.customerBL = customerBL;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = customerBL.GetMyProfile(userId);

            if (profile == null)
            {
                // Customer user exists but has no Customer entity yet (registered via Register page)
                // Redirect to their accounts page as a fallback
                TempData["Warning"] = "Your profile is not fully set up yet. Please contact a bank employee to complete your profile.";
                return RedirectToAction("Index", "CustomerAccount");
            }

            return View(profile);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = customerBL.GetMyProfile(userId);

            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(CustomerProfileVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var (success, errors) = await customerBL.UpdateMyProfile(userId, model);

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
    }
}
