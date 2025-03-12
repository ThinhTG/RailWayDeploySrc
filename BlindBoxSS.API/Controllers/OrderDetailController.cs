using Microsoft.AspNetCore.Mvc;
using Models;
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

    // Lấy chi tiết đơn hàng theo ID
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDetail>> GetById(Guid id)
    {
        var orderDetail = await _orderDetailService.GetOrderDetailByIdAsync(id);
        if (orderDetail == null) return NotFound();
        return Ok(orderDetail);
    }

    // Thêm chi tiết đơn hàng mới
    [HttpPost]
    public async Task<ActionResult<OrderDetail>> Create([FromBody] OrderDetail orderDetail)
    {
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
