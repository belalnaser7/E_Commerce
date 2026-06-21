using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Interfaces
{
    public interface IRepositoryCart
    {

        Task<Cart?> GetByUserIdAsync(string userId);

        Task<Cart?> GetByIdAsync(int cartId);

        Task AddAsync(Cart cart);
        void Remove(CartItem cart);

       
    }
}
