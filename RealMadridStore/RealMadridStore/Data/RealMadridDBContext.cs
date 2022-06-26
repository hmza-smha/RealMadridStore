using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealMadridStore.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Data
{
    public class RealMadridDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> products { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }


        public RealMadridDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Kit", Details = "Kit" },
                new Category { Id = 2, Name = "Training", Details = "Training" },
                new Category { Id = 3, Name = "A&G", Details = "Accessories & Gifts" }
              );            

            modelBuilder.Entity<Product>().HasData(
               new Product { Id = 1, Name = "Real Madrid Home Jersey", Description = "Real Madrid Home Jersey", ImageUrl = "~/images/RealMadrid.png", Price = 70, CategoryId = 1 },
               new Product { Id = 2, Name = "Real Madrid Home Jersey", Description = "Real Madrid Home Jersey", ImageUrl = "~/images/RealMadrid.png", Price = 12, CategoryId = 1 },
               new Product { Id = 3, Name = "Mens Training T-Shirt 22/23 Black", Description = "Mens Training T-Shirt 22/23 Black", ImageUrl = "~/images/TRAINING.png", Price = 80, CategoryId = 2 },
               new Product { Id = 4, Name = "Real Madrid Crest Logo Cap - Grey", Description = "Real Madrid Crest Logo Cap - Grey", ImageUrl = "~/images/A&G.png", Price = 30, CategoryId = 3 }
             );

            // any unique string id
            const string ADMIN_ID = "a18be9c0";
            const string ADMIN_ROLE_ID = "ad376a8f";

            const string EDITOR_ID = "a50ze710";
            const string EDITOR_ROLE_ID = "bd586a8f";

            // create an Admin role
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ADMIN_ROLE_ID,
                Name = "Admin",
                NormalizedName = "Admin"
            }, new IdentityRole
            {
                Id = EDITOR_ROLE_ID,
                Name = "Editor",
                NormalizedName = "Editor"
            });

            // create a User
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "Admin123#"),
                SecurityStamp = string.Empty
            },
            new ApplicationUser
            {
                Id = EDITOR_ID,
                UserName = "editor",
                NormalizedUserName = "editor",
                Email = "editor@gmail.com",
                NormalizedEmail = "editor@gmail.com",
                EmailConfirmed = false,
                PasswordHash = hasher.HashPassword(null, "Editor123#"),
                SecurityStamp = string.Empty
            });

            // assign that user to the admin role
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID
            },
            new IdentityUserRole<string>
            {
                RoleId = EDITOR_ROLE_ID,
                UserId = EDITOR_ID
            });
        }
    }
}
