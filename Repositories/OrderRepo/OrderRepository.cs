using DAO;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories.OrderRep
{
    public class OrderRepository : IOrderRepository
    {

        private readonly BlindBoxDbContext _context;

        public OrderRepository(BlindBoxDbContext context)
        {
            _context = context;
        }

        public IQueryable<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.Account) // Lấy thông tin tài khoản của khách hàng
                .Include(o => o.Account.Addresses) // Lấy danh sách địa chỉ của tài khoản (Không cần thiết nếu chỉ lấy 1 Address)
                .Include(o => o.DeliveryAddress) // Lấy thông tin địa chỉ đã chọn
                .AsQueryable();
        }

        public async Task<Order> AddAsync(Order order)
        {
            _context.Set<Order>().Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Set<Order>().FindAsync(id);
            if (order != null)
            {
                _context.Set<Order>().Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Set<Order>().FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Set<Order>().ToListAsync();
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        public IQueryable<Order> GetByAccountId(string id)
        {
            return _context.Orders.Where(o => o.AccountId == id);
        }
    }
}