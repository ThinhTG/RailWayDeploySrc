using DAO;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlindBoxDbContext _context;

        public CategoryRepository(BlindBoxDbContext context)
        {
            _context = context;
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Set<Category>().Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _context.Set<Category>().FindAsync(id);
            if (category != null)
            {
                _context.Set<Category>().Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public   IQueryable<Category> GetAll()
        {
            return _context.Category.AsQueryable();
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return await _context.Set<Category>().FindAsync(id);
        }

        public async Task<IEnumerable<Category>> GetCategorysAsync()
        {
            return await _context.Set<Category>().ToListAsync();
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> AddCategoryImage(Guid id,string Url)
        {
            var category = await _context.Set<Category>().FindAsync(id);
            category.CategoryImage = Url;
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
