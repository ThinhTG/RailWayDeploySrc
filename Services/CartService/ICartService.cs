using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICartService
    {
        Task AddToCart(CartDTO cartDto);
       Task<IEnumerable<Cart>> GetCartByUserId(string userId);
        Task<bool> UpdateCartItemQuantity(Guid cartId, string userId, int quantity);
        Task<bool> DeleteCartItem(Guid cartId);



    }
}
