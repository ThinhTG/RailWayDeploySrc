using Model.Models;

namespace Repository.OrderRepo
{
    public interface IOrderDetailRepo
    {
        public OrderDetail CreateOrderDetail(OrderDetail orderDetail);

        public List<OrderDetail> GetAllOrderDetails();

        public OrderDetail GetOrderDetailById(int id);

        public OrderDetail UpdateOrderDetail(OrderDetail orderDetail);

        public void RemoveOrderDetail(int id);

        public OrderDetail GetOrderDetailByOrderId(int orderId);

        public List<OrderDetail> GetOrderDetailByPackageId(int productId);

        public List<OrderDetail> GetOrderDetailByBlindBoxId(int blindBoxId);

        public List<OrderDetail> GetOrderDetailByUserId(int userId);

    }
}
