using BankSystem.BL;
using BankSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BranchController : Controller
    {
        private readonly IBranchBL branchBL;

        public BranchController(IBranchBL branchBL)
        {
            this.branchBL = branchBL;
        }

        public IActionResult Index()
        {
            var branches = branchBL.GetAllBranches();
            return View(branches);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BranchCreateVM model)
        {
            if (ModelState.IsValid)
            {
                branchBL.CreateBranch(model);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var branch = branchBL.GetBranchById(id);
            if (branch == null) return NotFound();
            return View(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BranchEditVM model)
        {
            if (ModelState.IsValid)
            {
                var success = branchBL.UpdateBranch(model);
                if (success) return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Failed to update branch.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(int id)
        {
            branchBL.ActivateBranch(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(int id)
        {
            branchBL.DeactivateBranch(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
