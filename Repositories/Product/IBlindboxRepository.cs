using DAO.Contracts;
using Models;
using Repositories.Pagging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Product
{
    public interface IBlindBoxRepository
    {
        Task<IEnumerable<BlindBox>> GetAllAsync();
        Task<BlindBox> GetByIdAsync(Guid? id);
        Task<BlindBox> AddAsync(BlindBox blindBox);
        Task<BlindBox> UpdateAsync(BlindBox blindBox);
        Task DeleteAsync(Guid id);
         
        Task<IEnumerable<BlindBox>> GetAllMobileAsync();
        IQueryable<BlindBox> GetAll();

        Task<List<BlindBox>> GetBlindBoxByTypeSell(string typeSell);

        Task<IEnumerable<BlindBox>> GetBlindBoxByTypeSellPaged(string typeSell);

        Task<IEnumerable<BlindBox>> GetBlindBoxLuckyWheel(Guid PackageId);


    }
}