using MovieSeriesManagement.Models.Entities;
using MovieSeriesManagement.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Services
{
    public interface IContentService
    {
        Task<IEnumerable<Content>> GetAllContentsAsync();
        Task<Content> GetContentByIdAsync(int id);
        Task<IEnumerable<Content>> SearchContentsAsync(ContentSearchViewModel searchModel);
        Task<IEnumerable<string>> GetAllGenresAsync();
        Task<IEnumerable<string>> GetAllPlatformsAsync();
        Task AddContentAsync(Content content);
        Task UpdateContentAsync(Content content);
        Task DeleteContentAsync(int id);
    }
}

