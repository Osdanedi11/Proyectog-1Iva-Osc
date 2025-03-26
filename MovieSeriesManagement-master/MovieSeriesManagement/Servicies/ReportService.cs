using Microsoft.EntityFrameworkCore;
using MovieSeriesManagement.Data;
using MovieSeriesManagement.Models.Entities;
using MovieSeriesManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserStatisticsViewModel> GetUserStatisticsAsync(string userId)
        {
            var userHistory = await _context.ViewingHistories
                .Include(vh => vh.Content)
                .Where(vh => vh.UserId == userId)
                .ToListAsync();

            var totalWatched = userHistory.Count;
            var moviesWatched = userHistory.Count(vh => vh.Content.Type == ContentType.Movie);
            var seriesWatched = userHistory.Count(vh => vh.Content.Type == ContentType.Series);

            var genreDistribution = userHistory
                .GroupBy(vh => vh.Content.Genre)
                .Select(g => new GenreStatisticsViewModel
                {
                    Genre = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var platformDistribution = userHistory
                .GroupBy(vh => vh.Content.Platform)
                .Select(g => new PlatformStatisticsViewModel
                {
                    Platform = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            return new UserStatisticsViewModel
            {
                TotalWatched = totalWatched,
                MoviesWatched = moviesWatched,
                SeriesWatched = seriesWatched,
                GenreDistribution = genreDistribution,
                PlatformDistribution = platformDistribution
            };
        }

        public async Task<IEnumerable<GenreStatisticsViewModel>> GetGenreStatisticsAsync()
        {
            var genreStats = await _context.Contents
                .GroupBy(c => c.Genre)
                .Select(g => new GenreStatisticsViewModel
                {
                    Genre = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return genreStats;
        }

        public async Task<IEnumerable<PlatformStatisticsViewModel>> GetPlatformStatisticsAsync()
        {
            var platformStats = await _context.Contents
                .GroupBy(c => c.Platform)
                .Select(g => new PlatformStatisticsViewModel
                {
                    Platform = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();

            return platformStats;
        }

        public async Task<IEnumerable<MonthlyStatisticsViewModel>> GetMonthlyStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var monthlyStats = await _context.ViewingHistories
                .Where(vh => vh.ViewDate >= startDate && vh.ViewDate <= endDate)
                .GroupBy(vh => new { Month = vh.ViewDate.Month, Year = vh.ViewDate.Year })
                .Select(g => new MonthlyStatisticsViewModel
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            return monthlyStats;
        }
    }
}

