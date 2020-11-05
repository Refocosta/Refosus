using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Refosus.Web.Data.Entities;

namespace Refosus.Web.Data
{
    public class DataContext : IdentityDbContext<UserEntity, RoleEntity, string>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        #region Parameters
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<DepartmentEntity> Deparments { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<CampusEntity> Campus { get; set; }
        public DbSet<CampusDetailsEntity> CampusDetails { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<MenuEntity> Menus { get; set; }
        public DbSet<RoleMenuEntity> RoleMenus { get; set; }
        public DbSet<DocumentTypeEntity> DocumentTypes { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        #endregion



        #region Messages
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<MessagetransactionEntity> MessagesTransaction { get; set; }
        public DbSet<MessageTypeEntity> MessagesTypes { get; set; }
        public DbSet<MessageBillStateEntity> MessagesBillState { get; set; }
        public DbSet<MessageStateEntity> MessagesStates { get; set; }
        public DbSet<MessageFileEntity> MessagesFile { get; set; }
        public DbSet<MessageCheckEntity> MessagesCheckes { get; set; }
        #endregion

        public DbSet<NewEntity> News { get; set; }
        public DbSet<CeCoEntity> CeCos { get; set; }


        #region Shopping
        public DbSet<ShoppingEntity> Shoppings { get; set; }
        public DbSet<ShoppingUnitEntity> ShoppingUnits { get; set; }
        public DbSet<ShoppingMeasureEntity> ShoppingMeasures { get; set; }
        public DbSet<ShoppingStateEntity> ShoppingStates { get; set; }
        public DbSet<ShoppingCategoryEntity> ShoppingCategories { get; set; }
        public DbSet<ShoppingTempItems> ShoppingTempItems { get; set; }
        public DbSet<ShoppingItemsEntity> ShoppingItems { get; set; }

        #endregion


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
