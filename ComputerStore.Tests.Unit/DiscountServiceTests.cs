using ComputerStore.Data.Db;
using ComputerStore.Data.Entities;
using ComputerStore.Service.DTOs;
using ComputerStore.Service.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ComputerStore.Tests.Unit
{
    public class DiscountServiceTests
    {
        private DiscountService CreateServiceWithTestData(out ComputerStoreDbContext context)
        {
            var options = new DbContextOptionsBuilder<ComputerStoreDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new ComputerStoreDbContext(options);

            var cpuCategory = new Category { Name = "CPU" };
            var product = new Product
            {
                Name = "Intel's Core i9-9900K",
                Price = 475.99m,
                Quantity = 5,
                Categories = new List<Category> { cpuCategory }
            };

            context.Categories.Add(cpuCategory);
            context.Products.Add(product);
            context.SaveChanges();

            return new DiscountService(context);
        }

        [Fact]
        public async Task Discount_Is_Applied_When_Quantity_Greater_Than_One()
        {
            var service = CreateServiceWithTestData(out _);
            var basket = new List<BasketItemDto>
            {
                new BasketItemDto { ProductName = "Intel's Core i9-9900K", Quantity = 2 }
            };

            var result = await service.CalculateDiscountAsync(basket);

            Assert.Equal(951.98m, result.OriginalTotal);
            Assert.Equal(928.18m, result.DiscountedTotal);
            Assert.Single(result.AppliedDiscounts);
        }

        [Fact]
        public async Task No_Discount_When_Quantity_Is_One()
        {
            var service = CreateServiceWithTestData(out _);
            var basket = new List<BasketItemDto>
            {
                new BasketItemDto { ProductName = "Intel's Core i9-9900K", Quantity = 1 }
            };

            var result = await service.CalculateDiscountAsync(basket);

            Assert.Equal(475.99m, result.OriginalTotal);
            Assert.Equal(475.99m, result.DiscountedTotal);
            Assert.Empty(result.AppliedDiscounts);
        }

        [Fact]
        public async Task Throws_When_Stock_Insufficient()
        {
            var service = CreateServiceWithTestData(out _);
            var basket = new List<BasketItemDto>
            {
                new BasketItemDto { ProductName = "Intel's Core i9-9900K", Quantity = 10 }
            };

            var ex = await Assert.ThrowsAsync<Exception>(() => service.CalculateDiscountAsync(basket));
            Assert.Contains("Not enough stock", ex.Message);
        }
    }
}
