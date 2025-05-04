using AutoMapper;
using ComputerStore.Data.Db;
using ComputerStore.Data.Entities;
using ComputerStore.Service.DTOs;
using ComputerStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Service.Services
{
    public class StockImportService : IStockImportService
    {
        private readonly ComputerStoreDbContext _context;
        private readonly IMapper _mapper;

        public StockImportService(ComputerStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task ImportAsync(List<ImportStockDto> stockItems)
        {
            foreach (var item in stockItems)
            {
                // Create any missing categories
                var categoryEntities = new List<Category>();
                foreach (var categoryName in item.Categories)
                {
                    var trimmed = categoryName.Trim();
                    var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == trimmed);
                    if (category == null)
                    {
                        category = new Category { Name = trimmed };
                        _context.Categories.Add(category);
                    }
                    categoryEntities.Add(category);
                }

                // Check if product exists
                var product = await _context.Products
                    .Include(p => p.Categories)
                    .FirstOrDefaultAsync(p => p.Name == item.Name);

                if (product == null)
                {
                    product = new Product
                    {
                        Name = item.Name,
                        Description = "", // Optional: fill if available
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Categories = categoryEntities
                    };
                    _context.Products.Add(product);
                }
                else
                {
                    // Update existing product’s quantity and categories
                    product.Quantity += item.Quantity;
                    product.Price = item.Price; // Optional: update latest price
                    product.Categories = categoryEntities;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
