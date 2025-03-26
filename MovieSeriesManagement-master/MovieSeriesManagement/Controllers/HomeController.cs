using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieSeriesManagement.Models;
using MovieSeriesManagement.Services;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecommendationService _recommendationService;
        private readonly IContentService _contentService;

        public HomeController(
            ILogger<HomeController> logger,
            IRecommendationService recommendationService,
            IContentService contentService)
        {
            _logger = logger;
            _recommendationService = recommendationService;
            _contentService = contentService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Get user ID if authenticated
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Get recommendations if user is authenticated
                if (!string.IsNullOrEmpty(userId))
                {
                    var recommendations = await _recommendationService.GetRecommendationsForUserAsync(userId, 6);
                    return View(recommendations);
                }

                // Otherwise, show some popular content
                var allContent = await _contentService.GetAllContentsAsync();
                return View(allContent);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la página de inicio");
                // En caso de error, mostrar todo el contenido
                var allContent = await _contentService.GetAllContentsAsync();
                return View(allContent);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

