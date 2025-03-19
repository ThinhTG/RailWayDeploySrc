    using Microsoft.AspNetCore.Mvc;
using Models;
using Services.DTO;
using Services.OrderS;
using Services.Product;

[Route("api/order-details")]
[ApiController]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailService _orderDetailService;
    private readonly IOrderService _orderService;
    private readonly IBlindBoxService _blindBoxService;
    private readonly IPackageService _packageService;

    public OrderDetailController(IOrderDetailService orderDetailService, IOrderService orderService, IBlindBoxService blindBoxService, IPackageService packageService )
    {
        _orderDetailService = orderDetailService;
        _orderService = orderService;
        _blindBoxService = blindBoxService;
        _packageService = packageService;
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
    //[HttpPost]
    //public async Task<ActionResult<OrderDetail>> Create([FromBody] CreateOrderDetailRequest orderDetailRequest)
    //{
    //    var orderDetail = new OrderDetail
    //    {
    //        OrderId = orderDetailRequest.OrderId,
    //        PackageId = orderDetailRequest.PackageId,
    //        BlindBoxId = orderDetailRequest.BlindBoxId,
    //        Quantity = orderDetailRequest.Quantity,
    //        Price = orderDetailRequest.Price
    //    };
    //    var createdOrderDetail = await _orderDetailService.AddOrderDetailAsync(orderDetail);
    //    return CreatedAtAction(nameof(GetById), new { id = createdOrderDetail.OrderDetailId }, createdOrderDetail);
    //}
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDetail>> CreateOrderFromBody([FromBody] CreateOrderDetailRequest orderDetailRequest)
    {
        try
        {
            // Validate OrderId
            var orderExists = await _orderService.GetByIdAsync(orderDetailRequest.OrderId);
            if (orderExists == null)
            {
                return BadRequest(new
                {
                    title = "Bad Request",
                    statusCode = 400,
                    message = $"Order with OrderId {orderDetailRequest.OrderId} does not exist."
                });
            }

            // Validate BlindBoxId (if provided)
            if (orderDetailRequest.BlindBoxId.HasValue)
            {
                var blindBoxExists = await _blindBoxService.GetByIdAsync(orderDetailRequest.BlindBoxId.Value);
                if (blindBoxExists == null)
                {
                    return BadRequest(new
                    {
                        title = "Bad Request",
                        statusCode = 400,
                        message = $"BlindBox with BlindBoxId {orderDetailRequest.BlindBoxId} does not exist."
                    });
                }
            }

            // Validate PackageId (if provided)
            if (orderDetailRequest.PackageId.HasValue)
            {
                var packageExists = await _packageService.GetPackageByIdAsync(orderDetailRequest.PackageId.Value);
                if (packageExists == null)
                {
                    return BadRequest(new
                    {
                        title = "Bad Request",
                        statusCode = 400,
                        message = $"Package with PackageId {orderDetailRequest.PackageId} does not exist."
                    });
                }
            }

            var orderDetail = new OrderDetail
            {
                OrderId = orderDetailRequest.OrderId,
                PackageId = orderDetailRequest.PackageId,
                BlindBoxId = orderDetailRequest.BlindBoxId,
                ReviewId = orderDetailRequest.ReviewId ?? null,
                Quantity = orderDetailRequest.Quantity,
                Price = orderDetailRequest.Price,
            };

            var createdOrderDetail = await _orderDetailService.AddOrderDetailAsync(orderDetail);
            return CreatedAtAction(nameof(GetById), new { id = createdOrderDetail.OrderDetailId }, createdOrderDetail);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating the order detail: {ex.ToString()}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.ToString()}");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                title = "Internal Server Error",
                statusCode = 500,
                message = "An error occurred while creating the order detail. Please check the server logs for more details.",
                detail = ex.InnerException?.Message ?? ex.Message
            });
        }
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
