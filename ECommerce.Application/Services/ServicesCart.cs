using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Domain_Models;
using Mapster;

namespace ECommerce.Application.Services
{
    public class ServicesCart : IServicesCart
    {
        private readonly IRepositoryCart repositoryCart;
        private readonly IRepositoryProduct repositoryProduct;

        public ServicesCart(IRepositoryCart repositoryCart, IRepositoryProduct repositoryProduct)
        {
            this.repositoryCart = repositoryCart;
            this.repositoryProduct = repositoryProduct;
        }
        public Cart? GetByUserId(string userId) // helper
        {
            var cart = repositoryCart.GetByUserId(userId);
            if (cart is null)
            {
                return null;
            }
            return cart;
        }





        public bool AddToCart(string userId, AddToCartDto dto)
        {
            if (dto.Quantity <= 0)
                return false;

            var product = repositoryProduct.GetById(dto.ProductId);
            if (product is null)
            {
                return false;
            }
            var cart = GetByUserId(userId);
            if (cart is null)
            {
                cart = new Cart()
                {
                    UserId = userId
                };

                repositoryCart.Add(cart);
            }
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem is not null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.Price,
                });
            }
            repositoryCart.Save();

            return true;
        }

        public bool ClearCart(string userId)
        {
            var cart = GetByUserId(userId);
            if (cart is null)
            {
                return false;
            }

            cart.Items.Clear();

            repositoryCart.Save();
            return true;
        }

        public CartDto? GetCart(string userId)
        {
            var cart = GetByUserId(userId);
            if (cart is null)
            {
                return null;
            }

            return new CartDto
            {
                Items = cart.Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Price = i.Product.Price,
                    Quantity = i.Quantity
                }).ToList()
            };

        }

        public bool RemoveItem(string userId, int cartitemid)
        {
            var cart = GetByUserId(userId);
            if (cart is null)
            {
                return false;
            }

            var existingItem = cart.Items.FirstOrDefault(o => o.Id == cartitemid);///الحته ديه غالبا غلط لان المفروض انا اتاكد من ال id بتاع ال Cartitem مش ال product لما اجي امسح بسمح بال cartitemid مش المنتج
            if (existingItem is null)
            {
                return false;
            }

            repositoryCart.Remove(existingItem);
            repositoryCart.Save();
            return true;

        }

        public bool UpdateQuantity(string userId, UpdateCartItemDto dto)
        {
            if (dto.Quantity <= 0)
                return false;
            var cart = GetByUserId(userId);
            if (cart is null)
            {
                return false;
            }

            var existingItem = cart.Items.FirstOrDefault(o => o.Id == dto.cartitemid);
            if (existingItem is null)
            {
                return false;
            }
            existingItem.Quantity = dto.Quantity;
            repositoryCart.Save();
            return true;

        }
    }
}

