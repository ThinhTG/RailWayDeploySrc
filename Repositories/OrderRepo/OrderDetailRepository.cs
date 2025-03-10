using DAO;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OrderRep
{
    public class OrderDetailRepository : IOrderDetailRepository

    {
        private readonly BlindBoxDbContext _context;

        public OrderDetailRepository(BlindBoxDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailAsync()
        {
            return await _context.Set<OrderDetail>().ToListAsync();
        }
        public async Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id)
        {
            return await _context.Set<OrderDetail>().FindAsync(id);
        }
        public async Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.Set<OrderDetail>().Add(orderDetail);
            await _context.SaveChangesAsync();
            return orderDetail;
        }
        public async Task<OrderDetail?> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            var existingOrderDetail = await _context.Set<OrderDetail>().FindAsync(orderDetail.OrderDetailId);
            if (existingOrderDetail == null)
            {
                return null;
            }

            _context.Entry(existingOrderDetail).CurrentValues.SetValues(orderDetail);
            await _context.SaveChangesAsync();
            return existingOrderDetail;
        }
        public async Task<bool> DeleteOrderDetailAsync(Guid id)
        {
            var OrderDetail = await _context.Set<OrderDetail>().FindAsync(id);
            if (OrderDetail == null)
            {
                return false;
            }

            _context.Set<OrderDetail>().Remove(OrderDetail);
            await _context.SaveChangesAsync();
            return true;
        }





    }
}