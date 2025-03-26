using Microsoft.EntityFrameworkCore;
using MovieSeriesManagement.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Data.Repositories
{
    public class RecommendationRepository : Repository<Recommendation>, IRecommendationRepository
    {
        public RecommendationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Recommendation>> GetUserRecommendationsAsync(string userId, int count)
        {
            return await _dbSet
                .Include(r => r.Content)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Score)
                .Take(count)
                .ToListAsync();
        }

        public async Task DeleteUserRecommendationsAsync(string userId)
        {
            var recommendations = await _dbSet
                .Where(r => r.UserId == userId)
                .ToListAsync();

            _dbSet.RemoveRange(recommendations);
            await _context.SaveChangesAsync();
        }
    }

    public interface IRecommendationRepository : IRepository<Recommendation>
    {
        Task<IEnumerable<Recommendation>> GetUserRecommendationsAsync(string userId, int count);
        Task DeleteUserRecommendationsAsync(string userId);
    }
}

