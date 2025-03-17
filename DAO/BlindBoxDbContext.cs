
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

            // Thiết lập khóa chính cho bảng Review
            modelBuilder.Entity<Review>()
                .HasKey(r => r.ReviewId);

            //// ✅ Quan hệ: OrderDetail - Review (1-1)
            //modelBuilder.Entity<Review>()
            //    .HasOne(r => r.OrderDetail)
            //    .WithOne(od => od.Review)
            //    .HasForeignKey<OrderDetail>(od => od.ReviewId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //// ✅ Quan hệ: Account - Review (1-n)
            //modelBuilder.Entity<Review>()
            //    .HasOne(r => r.Account)
            //    .WithMany(a => a.Reviews)
            //    .HasForeignKey(r => r.AccountId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //// ✅ Quan hệ: BlindBox - Review (1-n)
            //modelBuilder.Entity<Review>()
            //    .HasOne(r => r.BlindBox)
            //    .WithMany(p => p.Reviews)
            //    .HasForeignKey(r => r.ProductId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
