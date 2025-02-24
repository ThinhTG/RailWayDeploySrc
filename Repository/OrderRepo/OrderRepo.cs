using DAO;
using Model.Models;

namespace Repository.OrderRepo
{
    public class OrderRepo : IOrderRepo
    {
        private readonly BlindBoxDBContext _context;

        public OrderRepo(BlindBoxDBContext context)
        {
            _context = context;
        }

        public Order CreateOrder(Order order)
        {
            if (order == null)
            {
                throw new System.Exception("Order is null");
            }
            else
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return order;
            }
        }

        public List<Order> GetAllOrders()
        {
            var orders = _context.Orders.ToList();
            return orders;
        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders.FirstOrDefault(p => p.OrderId == id);
            if (order == null)
            {
                throw new System.Exception("Order not found");
            }
            return order;
        }

        public Order UpdateOrder(Order order)
        {
            _context.Entry(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return order;
        }

        public void RemoveOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(p => p.OrderId == id);
            if (order == null)
            {
                throw new System.Exception("Order not found");
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public Order GetOrderByUserId(int userId)
        {
            var order = _context.Orders.FirstOrDefault(p => p.AccountId == userId);
            if (order == null)
            {
                throw new System.Exception("Order not found");
            }
            return order;
        }

        public Order GetOrderByStatus(string status)
        {
            var order = _context.Orders.FirstOrDefault(p => p.OrderStatus == status);
            if (order == null)
            {
                throw new System.Exception("Order not found");
            }
            return order;
        }

        public List<Order> GetOrderByDate(string date)
        {
            var orders = _context.Orders.Where(p => p. == date).ToList();
            if (orders == null)
            {
                throw new System.Exception("Order not found");
            }
            return orders;
        }
    }
}
