using RealMadridStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Services
{
    public interface IProduct
    {
        Task<List<Category>> GetCategories();

        Task<List<Product>> GetAllProducts();

        Task<Product> CreateProduct(Product Product, int categoryId);

        Task<List<Product>> GetProducts(int? CategoryId);

        Task<Product> GetProduct(int Id);

        Task<Product> UpdateProduct(int Id, Product Product);

        Task DeleteProduct(int Id);
    }
}
