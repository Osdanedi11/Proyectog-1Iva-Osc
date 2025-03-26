using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSeriesManagement.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        // Quitar el atributo Required para permitir valores nulos
        [StringLength(500)]
        public string ProfilePictureUrl { get; set; }

        // Navigation properties
        public virtual ICollection<ViewingHistory> ViewingHistories { get; set; } = new List<ViewingHistory>();
        public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
    }
}

