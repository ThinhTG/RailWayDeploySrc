using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.OrderS;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/revenue")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public RevenueController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("by-day")]
        public IActionResult GetRevenueByDay([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var revenue = _orderService.GetRevenueByDay(startDate, endDate);
            return Ok(revenue);
        }

        [HttpGet("by-month")]
        public IActionResult GetRevenueByMonth([FromQuery] int year)
        {
            var revenue = _orderService.GetRevenueByMonth(year);
            return Ok(revenue);
        }

        [HttpGet("by-year")]
        public IActionResult GetRevenueByYear()
        {
            var revenue = _orderService.GetRevenueByYear();
            return Ok(revenue);
        }
    }
}
