using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieSeriesManagement.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Controllers
{
    [Authorize]
    public class ViewingHistoryController : Controller
    {
        private readonly IViewingHistoryService _viewingHistoryService;
        private readonly IContentService _contentService;

        public ViewingHistoryController(
            IViewingHistoryService viewingHistoryService,
            IContentService contentService)
        {
            _viewingHistoryService = viewingHistoryService;
            _contentService = contentService;
        }

        // GET: ViewingHistory
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var history = await _viewingHistoryService.GetUserHistoryAsync(userId);
            return View(history);
        }

        // POST: ViewingHistory/Record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Record(int contentId, int progress, bool completed = false)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if content exists
            var content = await _contentService.GetContentByIdAsync(contentId);
            if (content == null)
            {
                return NotFound();
            }

            await _viewingHistoryService.RecordViewingAsync(userId, contentId, progress, completed);

            // Redirect back to content details
            return RedirectToAction("Details", "Content", new { id = contentId });
        }

        // POST: ViewingHistory/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _viewingHistoryService.DeleteViewingHistoryAsync(id, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}

