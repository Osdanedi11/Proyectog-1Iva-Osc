using System.ComponentModel.DataAnnotations;

namespace MovieSeriesManagement.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name = "Nombre completo")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Foto de perfil")]
        public string ProfilePictureUrl { get; set; }

        [Display(Name = "Roles")]
        public string Roles { get; set; }
    }
}

