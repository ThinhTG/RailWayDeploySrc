using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.DTO;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] CartDTO cartDto)
        {
            if (cartDto == null)
                return BadRequest("Invalid cart data");

            try
            {
                await _cartService.AddToCart(cartDto);
                return Ok(new { Message = "Item added to cart successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("get-cart/{userId}")]
        public async Task<IActionResult> GetCartByUserId(string userId)
        {
            try
            {
                var cartItems = await _cartService.GetCartByUserId(userId);
                return Ok(cartItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartItemDTO model)
        {
            try
            {
                bool result = await _cartService.UpdateCartItemQuantity(model.CartId, model.UserId, model.Quantity);
                if (result)
                    return Ok(new { message = "Cart item updated successfully." });
                else
                    return BadRequest(new { message = "Failed to update cart item." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }

        [HttpDelete("delete-cart/{cartId}")]
        public async Task<IActionResult> DeleteCartItem(Guid cartId)
        {
            try
            {
                bool isDeleted = await _cartService.DeleteCartItem(cartId);
                if (isDeleted)
                    return Ok(new { message = "Cart item deleted successfully." });
                else
                    return NotFound(new { message = "Cart item not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }


    }
}
