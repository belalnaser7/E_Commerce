using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Interfaces
{
    public interface IRepositoryCart
    {

        Cart? GetByUserId(string userId);

        Cart? GetById(int cartId);

        void Add(Cart cart);
        void Remove(CartItem cart);

        void Save();
    }
}
