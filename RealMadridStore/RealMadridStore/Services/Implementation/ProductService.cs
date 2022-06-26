using Microsoft.EntityFrameworkCore;
using RealMadridStore.Data;
using RealMadridStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Services
{
    public class ProductService : IProduct
    {
        private RealMadridDBContext _context;

        public ProductService(RealMadridDBContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProduct(Product Product, int categoryId)
        {
            Product.CategoryId = categoryId;
            _context.Entry(Product).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return Product;
        }

        public async Task DeleteProduct(int Id)
        {
            Product product = await _context.products.FindAsync(Id);
            _context.Entry(product).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.products.Include(x => x.category).ToListAsync();
        }

        public async Task<Product> GetProduct(int Id)
        {
            return await _context.products.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,                
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Description = p.Description,
                CategoryId = p.CategoryId
            }).FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<List<Product>> GetProducts(int? CategoryId)
        {
            return await _context.products.Where(x => x.CategoryId == CategoryId).ToListAsync();
        }

        public async Task<Product> UpdateProduct(int Id, Product Product)
        {
            Product UpdatedProduct = new Product
            {
                Id = Product.Id,
                Name = Product.Name,
                Price = Product.Price,
                ImageUrl = Product.ImageUrl,
                Description = Product.Description,
                CategoryId = Product.CategoryId
            };
            _context.Entry(UpdatedProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Product;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.categories.ToListAsync();
        }
    }
}
