using MovieSeriesManagement.Data.Repositories;
using MovieSeriesManagement.Models.Entities;
using MovieSeriesManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Services
{
    public class ViewingHistoryService : IViewingHistoryService
    {
        private readonly IViewingHistoryRepository _viewingHistoryRepository;
        private readonly IContentRepository _contentRepository;

        public ViewingHistoryService(
            IViewingHistoryRepository viewingHistoryRepository,
            IContentRepository contentRepository)
        {
            _viewingHistoryRepository = viewingHistoryRepository;
            _contentRepository = contentRepository;
        }

        public async Task<IEnumerable<ViewingHistoryViewModel>> GetUserHistoryAsync(string userId)
        {
            var history = await _viewingHistoryRepository.GetUserHistoryAsync(userId);

            return history.Select(h => new ViewingHistoryViewModel
            {
                Id = h.Id,
                ContentId = h.ContentId,
                ContentTitle = h.Content.Title,
                ContentType = h.Content.Type.ToString(),
                Progress = h.Progress,
                ViewDate = h.ViewDate,
                Completed = h.Completed,
                ImageUrl = h.Content.ImageUrl
            });
        }

        public async Task<ViewingHistory> GetUserContentHistoryAsync(string userId, int contentId)
        {
            return await _viewingHistoryRepository.GetUserContentHistoryAsync(userId, contentId);
        }

        public async Task RecordViewingAsync(string userId, int contentId, int progress, bool completed)
        {
            // Check if content exists
            var content = await _contentRepository.GetByIdAsync(contentId);
            if (content == null)
            {
                throw new ArgumentException("El contenido especificado no existe", nameof(contentId));
            }

            // Check if history entry already exists
            var existingHistory = await _viewingHistoryRepository.GetUserContentHistoryAsync(userId, contentId);

            if (existingHistory != null)
            {
                // Update existing entry
                existingHistory.Progress = progress;
                existingHistory.Completed = completed;
                existingHistory.ViewDate = DateTime.UtcNow;

                await _viewingHistoryRepository.UpdateAsync(existingHistory);
            }
            else
            {
                // Create new entry
                var newHistory = new ViewingHistory
                {
                    UserId = userId,
                    ContentId = contentId,
                    Progress = progress,
                    Completed = completed,
                    ViewDate = DateTime.UtcNow
                };

                await _viewingHistoryRepository.AddAsync(newHistory);
            }

            await _viewingHistoryRepository.SaveChangesAsync();
        }

        public async Task DeleteViewingHistoryAsync(int id, string userId)
        {
            var history = await _viewingHistoryRepository.GetByIdAsync(id);

            if (history == null || history.UserId != userId)
            {
                throw new ArgumentException("El historial especificado no existe o no pertenece al usuario", nameof(id));
            }

            await _viewingHistoryRepository.DeleteAsync(history);
            await _viewingHistoryRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ViewingHistory>> GetRecentlyWatchedAsync(string userId, int count)
        {
            return await _viewingHistoryRepository.GetRecentlyWatchedAsync(userId, count);
        }
    }
}

