
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DAO
{
    public class BlindBoxDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public BlindBoxDbContext(DbContextOptions<BlindBoxDbContext> options) : base(options) { }



        public DbSet<ApplicationUser> Accounts { get; set; }

        public DbSet<ApplicationRole> Roles { get; set; }

        public DbSet<BlindBox> BlindBoxes { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Package> Packages { get; set; }

        public DbSet<PackageImage> PackageImages { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<Wallet> Wallet { get; set; }

        public DbSet<WalletTransaction> WalletTransaction { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed dữ liệu Role
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );

            // Order - ApplicationUser relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Account)
                .WithMany()
                .HasForeignKey(o => o.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal precision for money-related fields
            modelBuilder.Entity<Order>()
                .Property(o => o.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.PriceTotal)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.DiscountMoney)
                .HasColumnType("decimal(18,2)");

            // Package - Category relationship
            modelBuilder.Entity<Package>()
                .HasOne(o => o.Category)
                .WithMany()
                .HasForeignKey(o => o.CategoryId);

            modelBuilder.Entity<BlindBox>()
           .HasOne(b => b.Package)
           .WithMany()
           .HasForeignKey(b => b.PackageId)
           .OnDelete(DeleteBehavior.NoAction); // Loại bỏ ON DELETE CASCADE

            modelBuilder.Entity<BlindBox>()
                .HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.NoAction); // Loại bỏ ON DELETE CASCADE


            // Thiết lập khóa chính cho Review
            modelBuilder.Entity<Review>()
                .HasKey(r => r.ReviewId);

            // ✅ Quan hệ: OrderDetail - Review (1-1)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.OrderDetail)
                .WithOne(od => od.Review)
                .HasForeignKey<OrderDetail>(od => od.OrderDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Quan hệ: Account - Review (1-n)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Account)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
             .HasOne(od => od.BlindBox)
                 .WithOne(b => b.OrderDetail)
                 .HasForeignKey<OrderDetail>(od => od.BlindBoxId) // Định rõ bên phụ thuộc
                 .OnDelete(DeleteBehavior.Cascade); // Chọn hành vi khi xóa BlindBox





        }
    }
}
