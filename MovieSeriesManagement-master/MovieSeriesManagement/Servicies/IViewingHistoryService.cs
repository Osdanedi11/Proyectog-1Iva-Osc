using MovieSeriesManagement.Models.Entities;
using MovieSeriesManagement.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Services
{
    public interface IViewingHistoryService
    {
        Task<IEnumerable<ViewingHistoryViewModel>> GetUserHistoryAsync(string userId);
        Task<ViewingHistory> GetUserContentHistoryAsync(string userId, int contentId);
        Task RecordViewingAsync(string userId, int contentId, int progress, bool completed);
        Task DeleteViewingHistoryAsync(int id, string userId);
        Task<IEnumerable<ViewingHistory>> GetRecentlyWatchedAsync(string userId, int count);
    }
}

