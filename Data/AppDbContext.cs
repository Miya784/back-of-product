using Admin.Models;
using customer.Models;
using Microsoft.EntityFrameworkCore;

namespace PosgresDb.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserAdmin> UsersAdmin { get; set; }
        public DbSet<ProductAdmin> ProductsAdmin { get; set; }
        public DbSet<UserCustomer> UsersCustomer { get; set; }
        public DbSet<HistoryCustomer> HistorysCustomer { get; set; }
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // create tables UserAdmin
            modelBuilder.Entity<UserAdmin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Email).IsRequired();
            });
            modelBuilder.Entity<UserAdmin>().HasData(
                new UserAdmin{
                    Id = 1,
                    Username = "admin",
                    Password = "admin",
                    Email = "admin@admin.burhan"
                });
            // create tables ProductAdmin
            modelBuilder.Entity<ProductAdmin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Stock).IsRequired();
            });
            modelBuilder.Entity<ProductAdmin>().HasData(
                new ProductAdmin
                {
                    Id = 1,
                    UserId = 1,
                    Name = "Product 1",
                    Description = "Description 1",
                    Price = 1000,
                    Stock = 1
                });
            // create tables UserCustomer
            modelBuilder.Entity<UserCustomer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Email).IsRequired();
            });
            modelBuilder.Entity<UserCustomer>().HasData(
                new UserCustomer
                {
                    Id = 1,
                    Username = "customer",
                    Password = "customer",
                    Email = "admin@customer.burhan"
                });
            // create tables HistoryCustomer
            modelBuilder.Entity<HistoryCustomer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).IsRequired();
            });
            modelBuilder.Entity<HistoryCustomer>().HasData(
                new HistoryCustomer
                {
                    Id = 1,
                    UserId = 1,
                    Name = "Product 1",
                    Description = "Description 1",
                    Price = 1000,
                    DateTime = "2021-01-01 00:00:00"
                });
        }
}
};
