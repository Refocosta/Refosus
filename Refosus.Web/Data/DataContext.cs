using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data.Entities;

namespace Refosus.Web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<DepartmentEntity> Deparments { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<CampusEntity> Campus { get; set; }
        public DbSet<CampusDetailsEntity> CampusDetails { get; set; }
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
        public DbSet<Refosus.Web.Data.Entities.CityEntity> CityEntity { get; set; }
    }
}
