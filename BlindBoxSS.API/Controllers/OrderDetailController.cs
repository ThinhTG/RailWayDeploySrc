using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.OrderS;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }
        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetAllOrderDetailAsync()
        {
            var OrderDetails = await _orderDetailService.GetAllOrderDetailAsync();
            return Ok(OrderDetails);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetById(Guid id)
        {
            var OrderDetails = await _orderDetailService.GetOrderDetailByIdAsync(id);
            if (OrderDetails == null)
            {
                return NotFound();
            }
            return Ok(OrderDetails);
        }
        [HttpPost]
        public async Task<ActionResult<BlindBox>> AddOrderDetailAsync(OrderDetail orderDetail)
        {
            var createdorderDetail = await _orderDetailService.AddOrderDetailAsync(orderDetail);
            return CreatedAtAction(nameof(GetById), new { id = createdorderDetail.OrderDetailId }, createdorderDetail);
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetailAsync(Guid id)
        {
            await _orderDetailService.DeleteOrderDetailAsync(id);
            return NoContent();
        }
    }

}