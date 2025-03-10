using BlindBoxSS.API.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Cache;
using Services.DTO;

namespace BlindBoxSS.API.Controllers
{
    [Route("cart-management")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IResponseCacheService _responseCacheService;

        public CartController(ICartService cartService, IResponseCacheService responseCacheService)
        {
            _cartService = cartService;
            _responseCacheService = responseCacheService;
        }

        [HttpPost("managed-carts/add-to-cart")]
        //[Authorize("UserPolicy")]
        public async Task<IActionResult> AddToCart([FromBody] CartDTO cartDto)
        {
            if (cartDto == null || string.IsNullOrWhiteSpace(cartDto.UserId))
                return BadRequest(new { Error = "Invalid cart data: UserId cannot be null or empty" });

            try
            {
                await _cartService.AddToCart(cartDto);
                await _responseCacheService.RemoveCacheResponseAsync($"/cart-management/managed-carts/{cartDto.UserId}");
                return Ok(new { Message = "Item added to cart successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

       
        [HttpGet("managed-carts/{userId}")]
        [Cache(1000)]
        //[Authorize("UserPolicy")]
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

        [HttpPut("managed-carts/update-quantity")]
        //[Authorize("UserPolicy")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartItemDTO model)
        {
            try
            {
                bool result = await _cartService.UpdateCartItemQuantity(model.CartId, model.UserId, model.Quantity);
                if (result)
                {
                    await _responseCacheService.RemoveCacheResponseAsync($"/cart-management/managed-carts/{model.UserId}"); 

                    return Ok(new { message = "Cart item updated successfully." });
                }
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

        [HttpDelete("managed-carts/delete/{cartId}")]
        //[Authorize("UserPolicy")]
        public async Task<IActionResult> DeleteCartItem(Guid cartId,Guid userId)
        {
            try
            {
                bool isDeleted = await _cartService.DeleteCartItem(cartId);
                if (isDeleted)
                {
                    await _responseCacheService.RemoveCacheResponseAsync($"/cart-management/managed-carts/{userId}"); 
                    return Ok(new { message = "Cart item deleted successfully." });
                }
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
