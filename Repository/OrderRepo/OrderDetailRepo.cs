using DAO;
using Model.Models;

namespace Repository.OrderRepo
{
    public class OrderDetailRepo : IOrderDetailRepo
    {
        private readonly BlindBoxDBContext _context;

        public OrderDetailRepo(BlindBoxDBContext context)
        {
            _context = context;
        }


        public OrderDetail CreateOrderDetail(OrderDetail orderDetail)
        {
            if (orderDetail == null)
            {
                throw new System.Exception("OrderDetail is null");
            }
            else
            {
                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();
                return orderDetail;
            }
        }

        public List<OrderDetail> GetAllOrderDetails()
        {

            var orderdetails = _context.OrderDetails.ToList();
            return orderdetails;
        }

        public OrderDetail GetOrderDetailById(int id)
        {
            var orderDetail = _context.OrderDetails.FirstOrDefault(p => p.OrderDetailId == id);
            if (orderDetail == null)
            {
                throw new System.Exception("OrderDetail not found");
            }
            return orderDetail;
        }

        public OrderDetail UpdateOrderDetail(OrderDetail orderDetail)
        {
            _context.Entry(orderDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return orderDetail;
        }

        public void RemoveOrderDetail(int id)
        {
            var orderDetail = _context.OrderDetails.FirstOrDefault(p => p.OrderDetailId == id);
            if (orderDetail == null)
            {
                throw new System.Exception("OrderDetail not found");
            }
            _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();
        }

        public OrderDetail GetOrderDetailByOrderId(int orderId)
        {
            var orderDetail = _context.OrderDetails.FirstOrDefault(p => p.OrderId == orderId);
            if (orderDetail == null)
            {
                throw new System.Exception("OrderDetail not found");
            }
            return orderDetail;
        }

        public List<OrderDetail> GetOrderDetailByPackageId(int productId)
        {
            var orderDetails = _context.OrderDetails.Where(p => p.PackageId == productId).ToList();
            if (orderDetails == null)
            {
                throw new System.Exception("OrderDetail not found");
            }
            return orderDetails;
        }


        public List<OrderDetail> GetOrderDetailByBlindBoxId(int blindboxId)
        {
            var orderDetails = _context.OrderDetails.Where(p => p.BlindBoxId == blindboxId).ToList();
            if (orderDetails == null)
            {
                throw new System.Exception("OrderDetail not found");
            }
            return orderDetails;
        }


        public List<OrderDetail> GetOrderDetailByUserId(int userId)
        {
            var orderDetails = _context.OrderDetails.Where(p => p.Order.AccountId == userId).ToList();
            if (orderDetails == null)
            {
                throw new System.Exception("OrderDetail not found");
            }
            return orderDetails;
        }

    }
}
