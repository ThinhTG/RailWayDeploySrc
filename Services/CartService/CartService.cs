using Repositories.UnitOfWork;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CartService : ICartService
    {
        public readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddToCart(CartDTO cartDto)
        {
       
                if (cartDto.Quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than zero.");

                var cartRepository = _unitOfWork.GetRepository<Cart>();

                // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
                var existingCartItem = await cartRepository.FindAsync(c => c.UserId == cartDto.UserId &&
                                                                            c.BlindBoxId == cartDto.BlindBoxId &&
                                                                            c.PackageId == cartDto.PackageId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += cartDto.Quantity;
                    await cartRepository.UpdateAsync(existingCartItem);
                }
                else
                {
                    var newCart = new Cart
                    {
                        CartId = Guid.NewGuid(), // Tạo mới GUID cho mỗi Cart
                        UserId = cartDto.UserId,
                        BlindBoxId = cartDto.BlindBoxId,
                        PackageId = cartDto.PackageId,
                        Quantity = cartDto.Quantity,
                        CreateDate = DateTime.UtcNow
                    };
                    await cartRepository.InsertAsync(newCart);
                }

                await _unitOfWork.SaveAsync();
            }
                public async Task<IEnumerable<Cart>> GetCartByUserId(string userId)
                {
                    var cartRepository = _unitOfWork.GetRepository<Cart>();

                    // Sử dụng phương thức FindListAsync để lấy danh sách giỏ hàng của người dùng
                    var carts = await cartRepository.FindListAsync(c => c.UserId == userId);

                    return carts;
                }

        public async Task<bool> UpdateCartItemQuantity(Guid cartId, string userId, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            var cartRepository = _unitOfWork.GetRepository<Cart>();

            // Tìm giỏ hàng của user
            var cartItem = await cartRepository.FindAsync(c => c.CartId == cartId && c.UserId == userId);

            if (cartItem == null)
                throw new KeyNotFoundException("Cart item not found.");

            if (quantity == 0)
            {
                // Xóa sản phẩm khỏi giỏ hàng nếu số lượng bằng 0
                await cartRepository.DeleteAsync(cartId);
            }
            else
            {
                cartItem.Quantity = quantity;
                await cartRepository.UpdateAsync(cartItem);
            }

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteCartItem(Guid cartId)
        {
           
            var cartRepository = _unitOfWork.GetRepository<Cart>();
            var cartItem = await cartRepository.GetByIdAsync(cartId);
            if (cartItem == null)
                return false;

            await cartRepository.DeleteAsync(cartId);
            await _unitOfWork.SaveAsync();
            return true;
        }

    }




}


