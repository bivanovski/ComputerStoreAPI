using AutoMapper;
using ComputerStore.Data.Db;
using ComputerStore.Data.Entities;
using ComputerStore.Service.DTOs;
using ComputerStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ComputerStore.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly ComputerStoreDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(ComputerStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _context.Products.Include(p => p.Categories).ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _context.Products.Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == id);
            return product == null ? null : _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            product.Categories = await _context.Categories
                .Where(c => dto.Categories.Contains(c.Name))
                .ToListAsync();

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto?> UpdateAsync(int id, CreateProductDto dto)
        {
            var product = await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return null;

            _mapper.Map(dto, product);

            product.Categories = await _context.Categories
                .Where(c => dto.Categories.Contains(c.Name))
                .ToListAsync();

            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
