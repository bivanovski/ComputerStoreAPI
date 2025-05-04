using ComputerStore.Data.Db;
using ComputerStore.Data.Entities;
using ComputerStore.Service.DTOs;
using ComputerStore.Service.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ComputerStore.Tests.Integration
{
    public class DiscountServiceIntegrationTests
    {
        private ComputerStoreDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ComputerStoreDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ComputerStoreDbContext(options);

            // Seed the database with test data
            SeedDatabase(context);

            return context;
        }

        private void SeedDatabase(ComputerStoreDbContext context)
        {
            var cpuCategory = new Category { Name = "CPU" };
            var gpuCategory = new Category { Name = "GPU" };

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Intel Core i9-9900K",
                    Price = 475.99m,
                    Quantity = 5,
                    Categories = new List<Category> { cpuCategory }
                },
                new Product
                {
                    Name = "NVIDIA RTX 3080",
                    Price = 699.99m,
                    Quantity = 3,
                    Categories = new List<Category> { gpuCategory }
                }
            };

            context.Categories.AddRange(cpuCategory, gpuCategory);
            context.Products.AddRange(products);
            context.SaveChanges();
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldApplyDiscountCorrectly()
        {
            // Arrange
            using var context = CreateDbContext();
            var discountService = new DiscountService(context);

            var basket = new List<BasketItemDto>
            {
                new BasketItemDto { ProductName = "Intel Core i9-9900K", Quantity = 2 },
                new BasketItemDto { ProductName = "NVIDIA RTX 3080", Quantity = 1 }
            };

            // Act
            var result = await discountService.CalculateDiscountAsync(basket);

            // Assert
            Assert.Equal(1651.97m, result.OriginalTotal); // 475.99*2 + 699.99
            Assert.Equal(1628.17m, result.DiscountedTotal); // 5% off 1 Intel Core i9-9900K
            Assert.Contains("5% off 1x Intel Core i9-9900K", result.AppliedDiscounts[0]);
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldThrowWhenProductNotFound()
        {
            // Arrange
            using var context = CreateDbContext();
            var discountService = new DiscountService(context);

            var basket = new List<BasketItemDto>
            {
                new BasketItemDto { ProductName = "Nonexistent Product", Quantity = 1 }
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => discountService.CalculateDiscountAsync(basket));
            Assert.Contains("Product 'Nonexistent Product' not found", ex.Message);
        }

        [Fact]
        public async Task CalculateDiscountAsync_ShouldThrowWhenStockInsufficient()
        {
            // Arrange
            using var context = CreateDbContext();
            var discountService = new DiscountService(context);

            var basket = new List<BasketItemDto>
            {
                new BasketItemDto { ProductName = "NVIDIA RTX 3080", Quantity = 5 }
            };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => discountService.CalculateDiscountAsync(basket));
            Assert.Contains("Not enough stock", ex.Message);
        }
    }
}
