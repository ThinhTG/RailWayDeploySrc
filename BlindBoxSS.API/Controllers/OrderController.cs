using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.OrderS;


namespace BlindBoxSS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;
        public OrderController(IOrderService service)
        {
            _orderService = service;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var order = await _orderService.GetAllAsync();
            return Ok(order);
        }
        [HttpGet("GetAll-paged")]
        public async Task<IActionResult> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _orderService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> Create([FromBody] UpdateOrderDto createDto)
        {
            // Map DTO to Entity
            var order = new Order
            {
                AccountId = createDto.AccountId,
                OrderStatus = createDto.OrderStatus,
                Price = createDto.Price,
                DeliveryAddress = createDto.DeliveryAddress,
                Note = createDto.Note,
                PhoneNumber = createDto.PhoneNumber,
                CreatedDate = DateTime.UtcNow // Auto-generated
            };

            var createdOrder = await _orderService.AddAsync(order);

            return CreatedAtAction(nameof(GetById), new { id = createdOrder.OrderId }, createdOrder);
        }

        [HttpPatch("{id}")] // update order status
        public async Task<IActionResult> PatchOrder(int id, [FromBody] UpdateOrderDto updateDto)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();

            if (updateDto.OrderStatus != null) order.OrderStatus = updateDto.OrderStatus;
            if (updateDto.PriceTotal > 0) order.PriceTotal = updateDto.PriceTotal;

            await _orderService.UpdateAsync(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteAsync(id);
            return NoContent();
        }
    }
}