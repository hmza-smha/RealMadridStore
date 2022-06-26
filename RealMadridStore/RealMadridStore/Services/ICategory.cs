using RealMadridStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealMadridStore.Services
{
    public interface ICategory
    {
        Task<Category> CreateCategory(Category Category);

        Task<List<Category>> GetCategories();

        Task<Category> GetCategory(int Id);

        Task<Category> UpdateCategory(int Id, Category Category);

        Task DeleteCategory(int Id);
    }
}
