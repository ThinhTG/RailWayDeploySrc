    using Microsoft.AspNetCore.Mvc;
using Models;
using Services.DTO;
using Services.OrderS;

[Route("api/order-details")]
[ApiController]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailService _orderDetailService;

    public OrderDetailController(IOrderDetailService orderDetailService)
    {
        _orderDetailService = orderDetailService;
    }

    // Lấy tất cả chi tiết đơn hàng
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDetail>>> GetAllAsync()
    {
        var orderDetails = await _orderDetailService.GetAllOrderDetailAsync();
        return Ok(orderDetails);
    }


    /// <summary>
    /// Lấy chi tiết đơn hàng theo Detail ID
    /// </summary>
    /// <param name="id">order detail Id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDetail>> GetById(Guid id)
    {
        var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
        if (orderDetail == null) return NotFound();
        return Ok(orderDetail);
    }

    /// <summary>
    /// Lấy OrderDetails theo OrderId
    /// </summary>
    /// <param name="orderId">OrderId</param>
    /// <returns>List Order Details </returns>
    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<IEnumerable<OrderDetail>>> GetByOrderId(int orderId)
    {
        var orderDetails = await _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId);
        if (orderDetails == null || orderDetails.Count() == 0)
        {
            return NotFound();
        }
        return Ok(orderDetails);
    }

    // Thêm chi tiết đơn hàng mới
    [HttpPost]
    public async Task<ActionResult<OrderDetail>> Create([FromBody] CreateOrderDetailRequest orderDetailRequest)
    {
        var orderDetail = new OrderDetail
        {
            OrderId = orderDetailRequest.OrderId,
            PackageId = orderDetailRequest.PackageId,
            BlindBoxId = orderDetailRequest.BlindBoxId,
            Quantity = orderDetailRequest.Quantity,
            Price = orderDetailRequest.Price
        };
        var createdOrderDetail = await _orderDetailService.AddOrderDetailAsync(orderDetail);
        return CreatedAtAction(nameof(GetById), new { id = createdOrderDetail.OrderDetailId }, createdOrderDetail);
    }

    // Xóa chi tiết đơn hàng
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _orderDetailService.DeleteOrderDetailAsync(id);
        return NoContent();
    }

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateOrderDetailAsync(int id, [FromBody] OrderDetail orderDetail)
    //{
    //    if (id != orderDetail.OrderDetailId)
    //    {
    //        return BadRequest();
    //    }

    //    var updatedorderDetail = await _orderDetailService.UpdateOrderDetailAsync(orderDetail);
    //    if (updatedorderDetail == null)
    //    {
    //        return NotFound();
    //    }

    //    return Ok(updatedorderDetail);
    //}
}
