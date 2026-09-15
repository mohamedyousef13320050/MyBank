using BankSystem.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly IReportBL reportBL;

        public ReportController(IReportBL reportBL)
        {
            this.reportBL = reportBL;
        }

        public IActionResult Index()
        {
            var report = reportBL.GetDashboardReport();
            return View(report);
        }

        // JSON endpoint for Dashboard live stats
        public IActionResult Stats()
        {
            var report = reportBL.GetDashboardReport();
            return Json(new
            {
                employees    = report.TotalEmployees,
                branches     = report.TotalBranches,
                accounts     = report.TotalAccounts,
                transactions = report.TotalTransactions
            });
        }
    }
}
