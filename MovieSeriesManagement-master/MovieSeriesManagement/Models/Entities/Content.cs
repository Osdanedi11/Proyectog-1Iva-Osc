using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieSeriesManagement.Models.Entities
{
    public enum ContentType
    {
        Movie,
        Series
    }

    public class Content
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        [Required]
        [StringLength(50)]
        public string Platform { get; set; }

        [Required]
        public ContentType Type { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string ImageUrl { get; set; }

        // Navigation properties
        public virtual ICollection<ViewingHistory> ViewingHistories { get; set; } = new List<ViewingHistory>();
        public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
    }
}

