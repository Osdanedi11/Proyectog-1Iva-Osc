using MovieSeriesManagement.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSeriesManagement.Models.ViewModels
{
    public class ContentSearchViewModel
    {
        [Display(Name = "Buscar")]
        public string SearchTerm { get; set; }

        [Display(Name = "Género")]
        public string Genre { get; set; }

        [Display(Name = "Plataforma")]
        public string Platform { get; set; }

        [Display(Name = "Tipo")]
        public ContentType? ContentType { get; set; }

        // For dropdown lists
        public IEnumerable<string> Genres { get; set; } = new List<string>();
        public IEnumerable<string> Platforms { get; set; } = new List<string>();
    }
}
