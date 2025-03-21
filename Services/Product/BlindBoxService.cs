using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Pagging;
using Repositories.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Product
{
    public class BlindBoxService : IBlindBoxService
    {
        private readonly IBlindBoxRepository _repository;

        public BlindBoxService(IBlindBoxRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BlindBox>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<PaginatedList<BlindBox>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<BlindBox> blindBoxes = _repository.GetAll().AsQueryable();
            return await PaginatedList<BlindBox>.CreateAsync(blindBoxes, pageNumber, pageSize);
        }
        public async Task<BlindBox> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<BlindBox>> GetBlindboxeByTypeSell(string typeSell)
        {
            return await _repository.GetBlindBoxByTypeSell(typeSell);
        }

        public async Task<BlindBox> AddAsync(BlindBox blindBox)
        {
            return await _repository.AddAsync(blindBox);
        }

        public async Task<BlindBox> UpdateAsync(BlindBox blindBox)
        {
            return await _repository.UpdateAsync(blindBox);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<PaginatedList<BlindBox>> GetAllFilter(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            IQueryable<BlindBox> blindBoxes = _repository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(searchByCategory))
            {
                blindBoxes = blindBoxes
                    .Include(b => b.Category)
                    .Where(b => b.Category.CategoryName.Contains(searchByCategory));
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                blindBoxes = blindBoxes.Where(b => b.BlindBoxName.Contains(searchByName));
            }

            if (minPrice.HasValue)
            {
                blindBoxes = blindBoxes.Where(b => b.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                blindBoxes = blindBoxes.Where(b => b.Price <= maxPrice.Value);
            }

            return await PaginatedList<BlindBox>.CreateAsync(blindBoxes, pageNumber, pageSize);
        }

        public async Task<IEnumerable<BlindBox>> GetAllAsync(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice)
        {
            IQueryable<BlindBox> blindBoxes = _repository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(searchByCategory))
            {
                blindBoxes = blindBoxes
                    .Include(b => b.Category)
                    .Where(b => b.Category.CategoryName.Contains(searchByCategory));
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                blindBoxes = blindBoxes.Where(b => b.BlindBoxName.Contains(searchByName));
            }

            if (minPrice.HasValue)
            {
                blindBoxes = blindBoxes.Where(b => b.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                blindBoxes = blindBoxes.Where(b => b.Price <= maxPrice.Value);
            }

            return await blindBoxes.ToListAsync();
        }
    }
}
