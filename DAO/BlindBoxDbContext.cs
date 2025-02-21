﻿
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

        public DbSet<CartDetail> CartDetail { get; set; }

        public DbSet<Package> Packages { get; set; }

        public DbSet<PackageImage> PackageImages { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<Wallet> Wallet { get; set; }

        public DbSet<WalletTransaction> WalletTransaction { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed dữ liệu Role
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );


        }
    }
}
