﻿using DAO.Contracts;
using Models;
using Repositories.Pagging;
using Services.DTO;

namespace Services.Product
{
    public interface IBlindBoxService
    {
        Task<IEnumerable<BlindBoxMobileResponse>> GetAllAsync(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, string? size);
        Task<BlindBox> GetByIdAsync(Guid id);
        Task<BlindBox> AddAsync(AddBlindBoxDTO addBlindBoxDTO);
        Task<BlindBox> UpdateAsync(Guid id, UpdateBlindBoxDTO updateBlindBoxDTO);
        Task DeleteAsync(Guid id);

        Task<PaginatedList<BlindBox>> GetAll(int pageNumber, int pageSize);

        Task<List<BlindBox>> GetBlindboxeByTypeSell(string typeSell);

        Task<PaginatedList<BlindBox>> GetAllFilter(string? searchByCategory, string? typeSell, string? size , string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize);

       

    }
}
