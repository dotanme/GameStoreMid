using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GameStoreMid.Models;

namespace GameStoreMid.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Many to many Products to tags
            modelBuilder.Entity<ProductTag>()
               .HasKey(ps => new { ps.ProductID, ps.TagID });

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductTags)
                .WithOne()
                .HasForeignKey(pc => pc.ProductID);

            modelBuilder.Entity<Tag>()
                .HasMany(pc => pc.ProductTags)
                .WithOne()
                .HasForeignKey(pc => pc.TagID);
            
            //Many to many Products to Orders
            modelBuilder.Entity<ProductOrder>()
               .HasKey(po => new { po.ProductID, po.OrderID });

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductOrders)
                .WithOne()
                .HasForeignKey(o => o.ProductID);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.ProductOrders)
                .WithOne()
                .HasForeignKey(p => p.OrderID);


            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<GameStoreMid.Models.ApplicationUser> ApplicationUser { get; set; }

        public DbSet<GameStoreMid.Models.Product> Product { get; set; }

        public DbSet<GameStoreMid.Models.Tag> Tag { get; set; }

        public DbSet<GameStoreMid.Models.ProductTag> ProductTag { get; set; }

        public DbSet<GameStoreMid.Models.Deal> Deal { get; set; }

        public DbSet<GameStoreMid.Models.Review> Review { get; set; }
        
        public DbSet<GameStoreMid.Models.ClientOrder> OrderClient { get; set; }

        public DbSet<GameStoreMid.Models.ProductOrder> ProductOrder { get; set; }

        public DbSet<GameStoreMid.Models.BrowsingHistory> BrowsingHistory { get; set; }
    }
}
