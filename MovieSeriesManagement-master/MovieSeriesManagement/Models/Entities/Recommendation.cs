using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieSeriesManagement.Models.Entities
{
    public enum RecommendationType
    {
        GenreBased,
        PlatformBased,
        PopularityBased,
        SimilarContent
    }

    public class Recommendation
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ContentId { get; set; }

        [Required]
        public RecommendationType Type { get; set; }

        public double Score { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ContentId")]
        public virtual Content Content { get; set; }
    }
}

