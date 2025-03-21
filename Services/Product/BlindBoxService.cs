using AutoMapper;
using DAO.Contracts;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Pagging;
using Repositories.Product;
using Services.DTO;
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
        private readonly IMapper _mapper;

        public BlindBoxService(IBlindBoxRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

        public async Task<PaginatedList<BlindBox>> GetAllFilter(string? searchByCategory,string? typeSell, string?size , string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
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

            if(!string.IsNullOrEmpty(size))
            {
                blindBoxes = blindBoxes.Where(b => b.Size.Contains(size));
            }
            if(!string.IsNullOrEmpty(typeSell))
            {
                blindBoxes = blindBoxes.Where(b => b.TypeSell.Contains(typeSell));
            }



            return await PaginatedList<BlindBox>.CreateAsync(blindBoxes, pageNumber, pageSize);
        }


        //Mobile
        public async Task<IEnumerable<BlindBoxMobileResponse>> GetAllAsync(
            string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice,string? size)
        {
            IQueryable<BlindBox> blindBoxes = _repository.GetAll()
                .Include(b => b.Category)
                .Include(b => b.BlindBoxImages) // Include bảng chứa hình ảnh
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchByCategory))
            {
                blindBoxes = blindBoxes.Where(b => b.Category.CategoryName.Contains(searchByCategory));
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

            if(!string.IsNullOrEmpty(size))
            {
                blindBoxes = blindBoxes.Where(b => b.Size.Contains(size));
            }

            var blindBoxMobileResponses = await blindBoxes.Select(b => new BlindBoxMobileResponse
            {
                BlindBoxId = b.BlindBoxId,
                ImageUrl = b.BlindBoxImages.Select(img => img.ImageUrl).FirstOrDefault(),
                BlindBoxName = b.BlindBoxName,
                Price = b.Price,
                Description = b.Description,
                Stock = b.Stock
            }).ToListAsync();

            return blindBoxMobileResponses;
        }

        public async Task<PaginatedList<BlindBox>> GetBlindboxeByTypeSellPaged(string typeSell, int pageNumber, int pageSize)
        {
            IQueryable<BlindBox> blindBoxes = (await _repository.GetBlindBoxByTypeSell(typeSell)).AsQueryable();
            return await PaginatedList<BlindBox>.CreateAsync(blindBoxes, pageNumber, pageSize);
        }
    }
}
