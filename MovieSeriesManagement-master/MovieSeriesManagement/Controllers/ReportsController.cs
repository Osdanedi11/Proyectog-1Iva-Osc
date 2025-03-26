using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieSeriesManagement.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: Reports/UserStatistics
        public async Task<IActionResult> UserStatistics()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var statistics = await _reportService.GetUserStatisticsAsync(userId);
            return View(statistics);
        }

        // GET: Reports/GenreStatistics
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GenreStatistics()
        {
            var statistics = await _reportService.GetGenreStatisticsAsync();
            return View(statistics);
        }

        // GET: Reports/PlatformStatistics
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PlatformStatistics()
        {
            var statistics = await _reportService.GetPlatformStatisticsAsync();
            return View(statistics);
        }

        // GET: Reports/MonthlyStatistics
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MonthlyStatistics(DateTime? startDate, DateTime? endDate)
        {
            // Default to last 6 months if dates not provided
            if (!startDate.HasValue)
            {
                startDate = DateTime.UtcNow.AddMonths(-6);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.UtcNow;
            }

            var statistics = await _reportService.GetMonthlyStatisticsAsync(startDate.Value, endDate.Value);

            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

            return View(statistics);
        }
    }
}

