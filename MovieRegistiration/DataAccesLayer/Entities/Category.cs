using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRegistiration.DataAccesLayer.Entities
{
    public class Category
    {
        // Koleksiyonu başlatma
        public Category()
        {
            Movies = new List<Movie>();
        }

        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        // Navigation property
        public virtual List<Movie> Movies { get; set; }
    }
}
/*
 * list, ICollection, IQueryable, IEnumerable
 */