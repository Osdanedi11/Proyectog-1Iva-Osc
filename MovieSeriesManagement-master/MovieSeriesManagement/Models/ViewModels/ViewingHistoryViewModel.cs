using System;
using System.ComponentModel.DataAnnotations;

namespace MovieSeriesManagement.Models.ViewModels
{
    public class ViewingHistoryViewModel
    {
        public int Id { get; set; }

        public int ContentId { get; set; }

        [Display(Name = "Título")]
        public string ContentTitle { get; set; }

        [Display(Name = "Tipo")]
        public string ContentType { get; set; }

        [Display(Name = "Progreso")]
        [Range(0, 100)]
        public int Progress { get; set; }

        [Display(Name = "Fecha de visualización")]
        public DateTime ViewDate { get; set; }

        [Display(Name = "Completado")]
        public bool Completed { get; set; }

        [Display(Name = "Imagen")]
        public string ImageUrl { get; set; }
    }
}

