using MovieSeriesManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Services
{
    public interface IReportService
    {
        Task<UserStatisticsViewModel> GetUserStatisticsAsync(string userId);
        Task<IEnumerable<GenreStatisticsViewModel>> GetGenreStatisticsAsync();
        Task<IEnumerable<PlatformStatisticsViewModel>> GetPlatformStatisticsAsync();
        Task<IEnumerable<MonthlyStatisticsViewModel>> GetMonthlyStatisticsAsync(DateTime startDate, DateTime endDate);
    }
}

