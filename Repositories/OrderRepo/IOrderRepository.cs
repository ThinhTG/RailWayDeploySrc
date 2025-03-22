using Models;

namespace Repositories.OrderRep
{
    public interface IOrderRepository
    {

        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetByIdAsync(int id);
        Task<Order> AddAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task DeleteAsync(int id);
        IQueryable<Order> GetAll();

        IQueryable<Order> GetByAccountId(string id);
        IQueryable<Order> GetListOrderForCheck();
    }
}