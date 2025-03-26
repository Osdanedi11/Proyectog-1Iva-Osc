using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSeriesManagement.Models.Entities
{
    public class ViewingHistory
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ContentId { get; set; }

        [Range(0, 100)]
        public int Progress { get; set; }

        public DateTime ViewDate { get; set; }

        public bool Completed { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}

