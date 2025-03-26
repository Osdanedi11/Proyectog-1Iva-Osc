using System.Collections.Generic;

namespace MovieSeriesManagement.Models.ViewModels
{
    public class UserStatisticsViewModel
    {
        public int TotalWatched { get; set; }
        public int MoviesWatched { get; set; }
        public int SeriesWatched { get; set; }
        public IEnumerable<GenreStatisticsViewModel> GenreDistribution { get; set; } = new List<GenreStatisticsViewModel>();
        public IEnumerable<PlatformStatisticsViewModel> PlatformDistribution { get; set; } = new List<PlatformStatisticsViewModel>();
    }

    public class GenreStatisticsViewModel
    {
        public string Genre { get; set; }
        public int Count { get; set; }
    }

    public class PlatformStatisticsViewModel
    {
        public string Platform { get; set; }
        public int Count { get; set; }
    }

    public class MonthlyStatisticsViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public string MonthName => new System.Globalization.DateTimeFormatInfo().GetMonthName(Month);
    }
}

