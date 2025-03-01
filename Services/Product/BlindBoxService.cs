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
        public async Task<BlindBox> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<BlindBox> AddAsync(BlindBox blindBox)
        {
            return await _repository.AddAsync(blindBox);
        }

        public async Task<BlindBox> UpdateAsync(BlindBox blindBox)
        {
            return await _repository.UpdateAsync(blindBox);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
