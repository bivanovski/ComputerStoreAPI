using ComputerStore.Data.Db;
using ComputerStore.Service.DTOs;
using ComputerStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Service.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly ComputerStoreDbContext _context;

        public DiscountService(ComputerStoreDbContext context)
        {
            _context = context;
        }

        public async Task<BasketDiscountResultDto> CalculateDiscountAsync(List<BasketItemDto> basket)
        {
            var result = new BasketDiscountResultDto();

            foreach (var item in basket)
            {
                var product = await _context.Products
                    .Include(p => p.Categories)
                    .FirstOrDefaultAsync(p => p.Name == item.ProductName);

                if (product == null)
                    throw new Exception($"Product '{item.ProductName}' not found.");

                if (item.Quantity > product.Quantity)
                    throw new Exception($"Not enough stock for product '{item.ProductName}'. Requested: {item.Quantity}, In stock: {product.Quantity}");

                decimal productTotal = product.Price * item.Quantity;
                result.OriginalTotal += productTotal;

                if (item.Quantity > 1)
                {
                    // Only 1 copy of this product gets a 5% discount
                    decimal discountPerUnit = product.Price * 0.05m;
                    decimal discountedUnitPrice = product.Price - discountPerUnit;

                    // First unit discounted, others full price
                    decimal discountedTotal = discountedUnitPrice + (product.Price * (item.Quantity - 1));
                    result.DiscountedTotal += discountedTotal;

                    result.AppliedDiscounts.Add($"5% off 1x {product.Name} (-{Math.Round(discountPerUnit, 2):C})");
                }
                else
                {
                    result.DiscountedTotal += productTotal;
                }
            }

            // Round totals to 2 decimal places
            result.OriginalTotal = Math.Round(result.OriginalTotal, 2);
            result.DiscountedTotal = Math.Round(result.DiscountedTotal, 2);

            return result;
        }
    }
}
