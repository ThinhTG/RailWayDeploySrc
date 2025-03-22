using Models;
using Repositories.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderS
{
    public interface IOrderService
    {

        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<Order> AddAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task<PaginatedList<Order>> GetAll(int pageNumber, int pageSize);

        Task<PaginatedList<Order>> GetByAccountId(string accountId,int pageNumber,int pageSize);

        Task<PaginatedList<Order>> GetListOrderForCheck(int pageNumber, int pageSize);
    }
}