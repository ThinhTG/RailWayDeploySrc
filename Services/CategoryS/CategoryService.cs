using Microsoft.AspNetCore.Http.HttpResults;
using Models;
using Repositories.CategoryRepo;
using Repositories.Pagging;
using Services.DTO;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CategoryS
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async  Task<Category> AddPCategoryAsync(AddCategoryDTO createDto)
        {
            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = createDto.CategoryName,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async  Task DeleteCategoryAsync(Guid id)
        {
             await _categoryRepository.DeleteAsync(id);
        }

        public async Task<PaginatedList<Category>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<Category> category = _categoryRepository.GetAll().AsQueryable();
            return await PaginatedList<Category>.CreateAsync(category, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _categoryRepository.GetCategorysAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category?> UpdateCategoryAsync(Guid id, UpdateCategoryDTO updateDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id); // Use repository, not _categoryService
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }
            category.CategoryName = updateDto.CategoryName;
            category.UpdatedAt = DateTime.UtcNow;
            await _categoryRepository.UpdateAsync(category);
            return category;

        }
    }
}
