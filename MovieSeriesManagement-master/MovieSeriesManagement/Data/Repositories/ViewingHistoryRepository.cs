using Microsoft.EntityFrameworkCore;
using MovieSeriesManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Data.Repositories
{
    public class ViewingHistoryRepository : Repository<ViewingHistory>, IViewingHistoryRepository
    {
        public ViewingHistoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ViewingHistory>> GetUserHistoryAsync(string userId)
        {
            return await _dbSet
                .Include(vh => vh.Content)
                .Where(vh => vh.UserId == userId)
                .OrderByDescending(vh => vh.ViewDate)
                .ToListAsync();
        }

        public async Task<ViewingHistory> GetUserContentHistoryAsync(string userId, int contentId)
        {
            return await _dbSet
                .Where(vh => vh.UserId == userId && vh.ContentId == contentId)
                .FirstOrDefaultAsync();
        }

        public async Task<Dictionary<string, int>> GetUserGenrePreferencesAsync(string userId)
        {
            var userHistory = await _dbSet
                .Include(vh => vh.Content)
                .Where(vh => vh.UserId == userId)
                .ToListAsync();

            return userHistory
                .GroupBy(vh => vh.Content.Genre)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count()
                );
        }

        public async Task<IEnumerable<ViewingHistory>> GetRecentlyWatchedAsync(string userId, int count)
        {
            return await _dbSet
                .Include(vh => vh.Content)
                .Where(vh => vh.UserId == userId)
                .OrderByDescending(vh => vh.ViewDate)
                .Take(count)
                .ToListAsync();
        }
    }

    public interface IViewingHistoryRepository : IRepository<ViewingHistory>
    {
        Task<IEnumerable<ViewingHistory>> GetUserHistoryAsync(string userId);
        Task<ViewingHistory> GetUserContentHistoryAsync(string userId, int contentId);
        Task<Dictionary<string, int>> GetUserGenrePreferencesAsync(string userId);
        Task<IEnumerable<ViewingHistory>> GetRecentlyWatchedAsync(string userId, int count);
    }
}

