using Model.Models;

namespace Repository.CartRepo
{
    public interface ICartRepo
    {
        void AddCart(Cart cart);
        Cart UpdateCart(Cart cart);
        void RemoveCart(int id);
        Cart GetCartById(int id);
        Cart GetCartByUserId(int userId);
    }
}
