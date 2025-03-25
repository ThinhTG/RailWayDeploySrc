using Models;
using Models.Enum;

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
        List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate);
        List<Order> GetOrdersByYear(int year);
        List<Order> GetAllConfirmedOrders();
        Task<Order> UpdateOrderStatus(int orderId, OrderStatus orderStatus);
        Task<IEnumerable<Order>> GetListOrderConfirmed();
        Task<IEnumerable<Order>> GetListOrderDelivering();
        Task<IEnumerable<Order>> GetListOrderCompleted();
    }
}