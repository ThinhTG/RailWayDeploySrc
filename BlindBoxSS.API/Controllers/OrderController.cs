using Microsoft.AspNetCore.Mvc;
using Models;
using Services.OrderS;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // Lấy tất cả đơn hàng (dùng HTTP GET với collection resource, không cần "getAll")
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    // Lấy danh sách đơn hàng có phân trang
    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
    {
        var result = await _orderService.GetAll(pageNumber, pageSize);
        return Ok(result);
    }


    /// <summary>
    /// Lấy một đơn hàng theo  OrderID
    /// </summary>
    /// <param name="id">OrderId</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }


    /// <summary>
    /// lấy danh sách đơn hàng theo AccountId
    /// </summary>
    /// <param name="accountId">AccountId</param>
    /// <param name="pageNumber">Số Trang</param>
    /// <param name="pageSize">Số Đơn hàng trong 1 trang</param>
    /// <returns></returns>
    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByAccountId(string accountId, int pageNumber = 1, int pageSize = 10)
    {
        var orders = await _orderService.GetByAccountId(accountId, pageNumber, pageSize);
        return Ok(orders);
    }


    // Tạo đơn hàng mới
    [HttpPost]
    public async Task<ActionResult<Order>> Create([FromBody] UpdateOrderDto createDto)
    {
        var order = new Order
        {
            AccountId = createDto.AccountId,
            OrderStatus = Models.Enum.OrderStatus.PENDING,
            Price = createDto.Price,
            PriceTotal = createDto.PriceTotal,
            DeliveryAddress = createDto.DeliveryAddress,
            PaymentConfirmed = false,
            Note = createDto.Note,
            PhoneNumber = createDto.PhoneNumber,
            CreatedDate = DateTime.UtcNow
        };

        var createdOrder = await _orderService.AddAsync(order);

        return CreatedAtAction(nameof(GetById), new { id = createdOrder.OrderId }, createdOrder);
    }

    // Cập nhật trạng thái đơn hàng (Dùng HTTP PATCH cho cập nhật một phần)
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderDto updateDto)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();

        //if (updateDto.OrderStatus != null) order.OrderStatus = updateDto.OrderStatus;
        if (updateDto.PriceTotal > 0) order.PriceTotal = updateDto.PriceTotal;

        await _orderService.UpdateAsync(order);
        return NoContent();
    }

    // Xóa đơn hàng
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _orderService.DeleteAsync(id);
        return NoContent();
    }

    // Cập nhật đơn hàng
    [HttpPost("{orderId}/orderdetails")]
    public async Task<ActionResult<Order>> CreateOrderDetails(int orderId, [FromBody] List<OrderDetail> orderDetails)
    {
        var order = await _orderService.GetByIdAsync(orderId);
        if (order == null) return NotFound();

        foreach (var orderDetail in orderDetails)
        {
            order.OrderDetails.Add(orderDetail);
        }

        await _orderService.UpdateAsync(order);
        return Ok(order);
    }
}
