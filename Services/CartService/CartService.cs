using AutoMapper;
using DAO.Contracts;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.UnitOfWork;
using Services.DTO;

namespace Services
{
    public class CartService : ICartService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddToCart(CartDTO cartDto)
        {
            if (cartDto == null)
                throw new ArgumentNullException(nameof(cartDto), "Cart data cannot be null.");

            if (cartDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var cartRepository = _unitOfWork.GetRepository<Cart>();
            var blindBox = await _unitOfWork.GetRepository<BlindBox>().GetByIdAsync(cartDto.BlindBoxId);

            var existingCartItem = await cartRepository.FindAsync(c =>
                c.UserId == cartDto.UserId &&
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
                    CartId = Guid.NewGuid(),
                    UserId = cartDto.UserId,
                    BlindBoxId = cartDto.BlindBoxId,
                    PackageId = cartDto.PackageId,
                    Quantity = cartDto.Quantity,
                    CreateDate = DateTime.UtcNow,
                    BlindBox = blindBox
                };
                await cartRepository.InsertAsync(newCart);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId cannot be null or empty.");

            var cartRepository = _unitOfWork.GetRepository<Cart>();

            //var carts = await cartRepository.FindListAsync(c => c.UserId == userId);
            var carts = await cartRepository
                        .Query() // Lấy IQueryable<Cart>
                      .Include(c => c.BlindBox) // Load BlindBox
                      .Include(c => c.Package) // Load Package
                     .Where(c => c.UserId == userId)
                       .ToListAsync(); // Chuyển thành List


            return carts ?? throw new KeyNotFoundException("User not found or cart is empty.");
        }

        public async Task<bool> UpdateCartItemQuantity(Guid cartId, string userId, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");

            var cartRepository = _unitOfWork.GetRepository<Cart>();
            var cartItem = await cartRepository.FindAsync(c => c.CartId == cartId && c.UserId == userId);

            if (cartItem == null)
                throw new KeyNotFoundException("Cart item not found.");

            if (quantity == 0)
            {
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
                throw new KeyNotFoundException("Cart item not found.");

            await cartRepository.DeleteAsync(cartId);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
