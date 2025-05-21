using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieRegistiration.DataAccesLayer.Entities;

namespace MovieRegistiration.DataAccesLayer.Context
{
    public class MovieContext : DbContext
    {
        // "MovieContext" adını kullanın (base'e parametre geçmeyin)
        public MovieContext() : base()
        {
            // Veritabanı başlatma stratejisi
            Database.SetInitializer(new CreateDatabaseIfNotExists<MovieContext>());
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Model yapılandırması - ilişkileri tanımlama
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Film ve Kategori arasındaki ilişkiyi açıkça tanımlama
            modelBuilder.Entity<Movie>()
                .HasRequired(m => m.Category)
                .WithMany(c => c.Movies)
                .HasForeignKey(m => m.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}