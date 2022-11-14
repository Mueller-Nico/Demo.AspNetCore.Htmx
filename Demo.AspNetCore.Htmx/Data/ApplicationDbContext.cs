
using Microsoft.EntityFrameworkCore;
using Demo.AspNetCore.Htmx.Models;


namespace Demo.AspNetCore.Htmx.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Model>(entity =>
            {

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Models_Categories");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK_Models_Manufacturers");

            });
        }


        public DbSet<Demo.AspNetCore.Htmx.Models.Category> Categories { get; set; }

        public DbSet<Demo.AspNetCore.Htmx.Models.Model> Models { get; set; }

        public DbSet<Demo.AspNetCore.Htmx.Models.Manufacturer> Manufacturers { get; set; }

    }
}