using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using simpleWebApp.Models;

namespace PosgresDb.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<UserCustomer> UserCustomers { get; set; }
        public virtual DbSet<ProductCustomer> ProductCustomers { get; set; }
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Email).IsRequired();
            });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin",
                    Email = "admin@than.com"
                });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.HasOne(e => e.User).WithMany(u => u.Products).HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Sample Product",
                    Description = "This is a sample product",
                    Price = 9.99m,
                    UserId = 1
                });
            modelBuilder.Entity<UserCustomer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });
            modelBuilder.Entity<UserCustomer>().HasData(
                new UserCustomer
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin",
                    Email = "admin@admin.com"
                });
            modelBuilder.Entity<ProductCustomer>(entity => {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Time).IsRequired();
                entity.HasOne(e => e.UserCustomer).WithMany(u => u.ProductCustomers).HasForeignKey(e => e.UserId);
            });
            modelBuilder.Entity<ProductCustomer>().HasData(
                new ProductCustomer
                {
                    Id = 1,
                    Name = "Sample Product",
                    Description = "This is a sample product",
                    Price = 9.99m,
                    Time = DateTime.Now,
                    UserId = 1
                });
        }
    }
}
