using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRegistiration.DataAccesLayer.Entities
{
    public class Movie
    {
        public int MovieId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int Duration { get; set; } // in minutes

        [StringLength(500)]
        public string Description { get; set; }

        // Tarih alanını nullable yaparak sorunları önleme
        public DateTime? ReleaseDate { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } // Navigation property
    }
}