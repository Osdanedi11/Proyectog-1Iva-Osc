using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSeriesManagement.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Data.Repositories
{
    public class ContentRepository : Repository<Content>, IContentRepository
    {
        private readonly ILogger<ContentRepository> _logger;

        public ContentRepository(ApplicationDbContext context, ILogger<ContentRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Content>> SearchContentAsync(string searchTerm, string genre, string platform, ContentType? type)
        {
            try
            {
                _logger.LogInformation($"Iniciando búsqueda con: searchTerm='{searchTerm}', genre='{genre}', platform='{platform}', type={type}");

                // Comenzar con todos los contenidos
                var query = _dbSet.AsQueryable();

                // Aplicar filtros solo si tienen valor
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    _logger.LogInformation($"Aplicando filtro por término de búsqueda: '{searchTerm}'");
                    query = query.Where(c => c.Title.Contains(searchTerm));
                }

                if (!string.IsNullOrWhiteSpace(genre))
                {
                    _logger.LogInformation($"Aplicando filtro por género: '{genre}'");
                    query = query.Where(c => c.Genre == genre);
                }

                if (!string.IsNullOrWhiteSpace(platform))
                {
                    _logger.LogInformation($"Aplicando filtro por plataforma: '{platform}'");
                    query = query.Where(c => c.Platform == platform);
                }

                if (type.HasValue)
                {
                    _logger.LogInformation($"Aplicando filtro por tipo: {type.Value}");
                    query = query.Where(c => c.Type == type.Value);
                }

                // Ejecutar la consulta y obtener los resultados
                var results = await query.ToListAsync();
                _logger.LogInformation($"Búsqueda completada. Resultados: {results.Count}");

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al realizar la búsqueda de contenidos");
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetAllGenresAsync()
        {
            return await _dbSet.Select(c => c.Genre).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllPlatformsAsync()
        {
            return await _dbSet.Select(c => c.Platform).Distinct().ToListAsync();
        }
    }

    public interface IContentRepository : IRepository<Content>
    {
        Task<IEnumerable<Content>> SearchContentAsync(string searchTerm, string genre, string platform, ContentType? type);
        Task<IEnumerable<string>> GetAllGenresAsync();
        Task<IEnumerable<string>> GetAllPlatformsAsync();
    }
}

