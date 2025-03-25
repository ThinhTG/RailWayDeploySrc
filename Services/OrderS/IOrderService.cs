using Models;
using Models.Enum;
using Repositories.Pagging;
using Services.DTO;
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

        List<RevenueReportDTO> GetRevenueByDay(DateTime startDate, DateTime endDate);
        List<RevenueReportDTO> GetRevenueByMonth(int year);
        List<RevenueReportDTO> GetRevenueByYear();
        Task<Order> UpdatePaymentConfirmed(int? orderCode, int orderId);
        Task<Order> UpdateOrderStatus(int orderId, OrderStatus orderStatus);
        Task<PaginatedList<Order>> GetListOrderDelivering( int pageNumber, int pageSize);
        Task<PaginatedList<Order>> GetListOrderCompleted(int pageNumber, int pageSize);
    }
}