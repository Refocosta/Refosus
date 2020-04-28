using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data.Entities;

namespace Refosus.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet <CompanyEntity> Companies { get; set; }
        public DbSet<Refosus.Web.Data.Entities.CampusEntity> CampusEntity { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CompanyEntity>()
                .HasIndex(t => t.Name)
                .IsUnique();
            modelBuilder.Entity<CompanyEntity>()
                .HasIndex(t => t.Code)
                .IsUnique();
        }
    }
}
