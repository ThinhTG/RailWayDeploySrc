using DAO;
using Model.Models;

namespace Repository.CartRepo
{
    public class CartRepo : ICartRepo
    {
        private readonly BlindBoxDBContext _context;

        public CartRepo(BlindBoxDBContext context)
        {
            _context = context;
        }

        void ICartRepo.AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        void ICartRepo.RemoveCart(int id)
        {
            var cartId = _context.Carts.FirstOrDefault(p => p.CartId == id);
            if (cartId == null)
            {
                throw new System.Exception("Cart not found");
            }
            _context.Carts.Remove(cartId);
            _context.SaveChanges();
        }

        Cart ICartRepo.GetCartById(int id)
        {
            var cartId = _context.Carts.FirstOrDefault(p => p.CartId == id);
            if (cartId == null)
            {
                throw new System.Exception("Cart not found");
            }
            return cartId;
        }

        Cart ICartRepo.GetCartByUserId(int userId)
        {
            var cart = _context.Carts.FirstOrDefault(p => p.AccountId == userId);
            if (cart == null)
            {
                throw new System.Exception("Cart not found");
            }
            return cart;
        }

        Cart ICartRepo.UpdateCart(Cart cart)
        {
            _context.Entry(cart).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return cart;
        }
    }
}
