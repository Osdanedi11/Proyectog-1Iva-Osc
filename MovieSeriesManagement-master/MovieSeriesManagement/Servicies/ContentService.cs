using Microsoft.Extensions.Logging;
using MovieSeriesManagement.Data.Repositories;
using MovieSeriesManagement.Models.Entities;
using MovieSeriesManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Services
{
    public class ContentService : IContentService
    {
        private readonly IContentRepository _contentRepository;
        private readonly ILogger<ContentService> _logger;

        public ContentService(IContentRepository contentRepository, ILogger<ContentService> logger)
        {
            _contentRepository = contentRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Content>> GetAllContentsAsync()
        {
            return await _contentRepository.GetAllAsync();
        }

        public async Task<Content> GetContentByIdAsync(int id)
        {
            return await _contentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Content>> SearchContentsAsync(ContentSearchViewModel searchModel)
        {
            try
            {
                if (searchModel == null)
                {
                    _logger.LogWarning("SearchModel es nulo, devolviendo todos los contenidos");
                    return await _contentRepository.GetAllAsync();
                }

                _logger.LogInformation($"Buscando contenidos con: SearchTerm='{searchModel.SearchTerm}', Genre='{searchModel.Genre}', Platform='{searchModel.Platform}', ContentType={searchModel.ContentType}");

                var results = await _contentRepository.SearchContentAsync(
                    searchModel.SearchTerm,
                    searchModel.Genre,
                    searchModel.Platform,
                    searchModel.ContentType
                );

                _logger.LogInformation($"Resultados encontrados: {results.Count()}");
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar contenidos");
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetAllGenresAsync()
        {
            return await _contentRepository.GetAllGenresAsync();
        }

        public async Task<IEnumerable<string>> GetAllPlatformsAsync()
        {
            return await _contentRepository.GetAllPlatformsAsync();
        }

        public async Task AddContentAsync(Content content)
        {
            await _contentRepository.AddAsync(content);
            await _contentRepository.SaveChangesAsync();
        }

        public async Task UpdateContentAsync(Content content)
        {
            await _contentRepository.UpdateAsync(content);
            await _contentRepository.SaveChangesAsync();
        }

        public async Task DeleteContentAsync(int id)
        {
            var content = await _contentRepository.GetByIdAsync(id);
            if (content != null)
            {
                await _contentRepository.DeleteAsync(content);
                await _contentRepository.SaveChangesAsync();
            }
        }
    }
}

