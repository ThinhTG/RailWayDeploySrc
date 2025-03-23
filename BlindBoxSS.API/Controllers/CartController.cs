using BlindBoxSS.API.Attributes;
using Microsoft.AspNetCore.Authorization;
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
            await _cartService.AddToCart(cartDto);
            await _responseCacheService.RemoveCacheResponseAsync($"/cart-management/managed-carts/{cartDto.UserId}");
            return Ok(new { Message = "Item added to cart successfully" });
        }

        [HttpGet("managed-carts/{userId}")]
        [Cache(10000)]
        //[Authorize("UserPolicy")]
        public async Task<IActionResult> GetCartByUserId(string userId)
        {
            var cartItems = await _cartService.GetCartByUserId(userId);
            return Ok(cartItems);
        }

        [HttpPut("managed-carts/update-quantity")]
        //[Authorize("UserPolicy")]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartItemDTO model)
        {
            await _cartService.UpdateCartItemQuantity(model.CartId, model.UserId, model.Quantity);
            await _responseCacheService.RemoveCacheResponseAsync($"/cart-management/managed-carts/{model.UserId}");
            return Ok(new { message = "Cart item updated successfully." });
        }

        [HttpDelete("managed-carts/delete/{cartId}")]
        //[Authorize("UserPolicy")]
        public async Task<IActionResult> DeleteCartItem(Guid cartId, Guid userId)
        {
            await _cartService.DeleteCartItem(cartId);
            await _responseCacheService.RemoveCacheResponseAsync($"/cart-management/managed-carts/{userId}");
            return Ok(new { message = "Cart item deleted successfully." });
        }
    }
}
