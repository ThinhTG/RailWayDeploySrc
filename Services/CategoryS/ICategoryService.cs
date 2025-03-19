using Models;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CategoryS
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoryAsync();
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<Category> AddPCategoryAsync(AddCategoryDTO createDto);
        Task<Category?> UpdateCategoryAsync(Guid id, UpdateCategoryDTO updateDto);
        Task DeleteCategoryAsync(Guid id);
        Task<PaginatedList<Category>> GetAll(int pageNumber, int pageSize);

        Task<Category> AddCategoryImage(Guid id, string Url);
    }
}
