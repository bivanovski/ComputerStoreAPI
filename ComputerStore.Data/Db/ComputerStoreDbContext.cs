using ComputerStore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Data.Db
{
   public  class ComputerStoreDbContext : DbContext
    {
        public ComputerStoreDbContext(DbContextOptions<ComputerStoreDbContext> options) : base(options) { }

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            modelBuilder.Entity<Category>()
                .Property(c => c.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Product>()
                 .Property(p => p.Name)
                 .IsRequired()
                 .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                  .Property(p => p.Price)
                  .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                  .HasMany(p => p.Categories)
                  .WithMany(c => c.Products);
            modelBuilder.Entity<Product>()
                   .Property(p => p.Quantity)
                   .HasDefaultValue(0);

            base.OnModelCreating(modelBuilder);
        }

    }
}
