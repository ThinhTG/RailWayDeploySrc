using Model.Models;

namespace Repository.CartRepo
{
    public interface ICartDetailRepo
    {
        int GetCount();
        CartDetail GetCartDetailById(int id);
        CartDetail UpdateCartDetailById(int id, CartDetail cartDetail);
    }
}
