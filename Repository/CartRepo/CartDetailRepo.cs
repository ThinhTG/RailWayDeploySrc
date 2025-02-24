using Model.Models;

namespace Repository.CartRepo
{
    public class CartDetailRepo : ICartDetailRepo
    {
        CartDetail ICartDetailRepo.GetCartDetailById(int id)
        {
            throw new NotImplementedException();
        }

        int ICartDetailRepo.GetCount()
        {
            throw new NotImplementedException();
        }

        CartDetail ICartDetailRepo.UpdateCartDetailById(int id, CartDetail cartDetail)
        {
            throw new NotImplementedException();
        }
    }
}
