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
            // Kiểm tra đầu vào
            if (cartDto == null)
                throw new ArgumentNullException(nameof(cartDto), "Cart data cannot be null.");

            if (string.IsNullOrEmpty(cartDto.UserId))
                throw new ArgumentException("UserId cannot be null or empty.");

            if (cartDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (cartDto.BlindBoxId == null && cartDto.PackageId == null)
                throw new ArgumentException("Either BlindBoxId or PackageId must be provided.");

            // Lấy repository
            var cartRepository = _unitOfWork.GetRepository<Cart>();

            // Kiểm tra BlindBox hoặc Package nếu có
            BlindBox? blindBox = null;
            Package? package = null;

            if (cartDto.BlindBoxId.HasValue)
            {
                blindBox = await _unitOfWork.GetRepository<BlindBox>().GetByIdAsync(cartDto.BlindBoxId.Value);
                if (blindBox == null)
                    throw new ArgumentException($"BlindBox with ID {cartDto.BlindBoxId} not found.");
            }

            if (cartDto.PackageId.HasValue)
            {
                package = await _unitOfWork.GetRepository<Package>().GetByIdAsync(cartDto.PackageId.Value);
                if (package == null)
                    throw new ArgumentException($"Package with ID {cartDto.PackageId} not found.");
            }

            // Tìm cart item hiện có
            var existingCartItem = await cartRepository.FindAsync(c =>
                c.UserId == cartDto.UserId &&
                ((cartDto.BlindBoxId.HasValue && c.BlindBoxId == cartDto.BlindBoxId) ||
                 (cartDto.PackageId.HasValue && c.PackageId == cartDto.PackageId)));

            if (existingCartItem != null)
            {
                // Cập nhật số lượng nếu đã tồn tại
                existingCartItem.Quantity += cartDto.Quantity;
                await cartRepository.UpdateAsync(existingCartItem);
            }
            else
            {
                // Tạo mới cart item
                var newCart = new Cart
                {
                    CartId = Guid.NewGuid(),
                    UserId = cartDto.UserId,
                    BlindBoxId = cartDto.BlindBoxId,
                    PackageId = cartDto.PackageId,
                    Quantity = cartDto.Quantity,
                    CreateDate = DateTime.UtcNow,
                    BlindBox = blindBox,
                    Package = package
                };
                await cartRepository.InsertAsync(newCart);
            }

            // Lưu thay đổi vào DB
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId cannot be null or empty.");

            var cartRepository = _unitOfWork.GetRepository<Cart>();

            //var carts = await cartRepository.FindListAsync(c => c.UserId == userId);
            var carts = await cartRepository
                        .Query() 
                      .Include(c => c.BlindBox)
                      .Include(c => c.Package)
                     .Where(c => c.UserId == userId)
                       .ToListAsync();


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
