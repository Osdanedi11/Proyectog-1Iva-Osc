using Microsoft.AspNetCore.Http;
using MovieSeriesManagement.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieSeriesManagement.Models.ViewModels
{
    public class ContentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [StringLength(50, ErrorMessage = "El género no puede exceder los 50 caracteres")]
        [Display(Name = "Género")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "La plataforma es obligatoria")]
        [StringLength(50, ErrorMessage = "La plataforma no puede exceder los 50 caracteres")]
        [Display(Name = "Plataforma")]
        public string Platform { get; set; }

        [Required(ErrorMessage = "El tipo de contenido es obligatorio")]
        [Display(Name = "Tipo")]
        public ContentType Type { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Fecha de lanzamiento")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "URL de la imagen")]
        public string ImageUrl { get; set; }
    }
}

