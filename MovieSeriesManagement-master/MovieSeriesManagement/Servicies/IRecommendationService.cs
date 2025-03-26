using MovieSeriesManagement.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Services
{
    public interface IRecommendationService
    {
        Task<IEnumerable<Content>> GetRecommendationsForUserAsync(string userId, int count = 10);
        Task GenerateRecommendationsAsync(string userId);
        Task<IEnumerable<Content>> GetSimilarContentsAsync(int contentId, int count = 5);
    }
}

