using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
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
        public Result<Cart?> GetByUserId(string userId) // helper
        {
            var cart = repositoryCart.GetByUserId(userId);
            if (cart is null)
            {
                return Result<Cart?>.Fail("The Cart isn't Exsit", ErrorType.NotFound);
            }
            return Result<Cart?>.Success(cart);
        }

        public Result AddToCart(string userId, AddToCartDto dto)
        {
            if (dto.Quantity <= 0)
                return Result.Fail("The Product Quantity Invalid");

            var product = repositoryProduct.GetById(dto.ProductId);
            if (product is null)
            {
                return Result.Fail("The Product isn't Exsit");
            }
            var cartResult = GetByUserId(userId);
            Cart cart;

            if (!cartResult.IsSuccess)
            {
               
                cart = new Cart()
                {
                    UserId = userId
                };

                repositoryCart.Add(cart);
            }
            var existingItem = cartResult.Data.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem is not null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cartResult.Data.Items.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    UnitPrice = product.Price,
                });
            }
            repositoryCart.Save();

            return Result.Success();
        }

        public Result ClearCart(string userId)
        {
            var cart = GetByUserId(userId);
            if (!cart.IsSuccess)
            {
                return cart;
            }

            cart.Data.Items.Clear();

            repositoryCart.Save();
            return Result.Success();
        }

        public Result<CartDto?> GetCart(string userId)
        {
            var cart = GetByUserId(userId);
            if (!cart.IsSuccess)
            {
                return Result<CartDto?>.Fail("The Cart isn't Exsit", ErrorType.NotFound);
            }

            //var o = cart.Data.Items.Select(i => new CartItemDto
            //{
            //    ProductId = i.ProductId,
            //    ProductName = i.Product.Name,
            //    Price = i.Product.Price,
            //    Quantity = i.Quantity
            //}).ToList();

            //var jj = new CartDto
            //{
            //    Items = o
            //};
            //return Result<CartDto?>.Success(jj);

            return Result<CartDto?>.Success(
                new CartDto
                {
                    Items = cart.Data.Items.Select(i => new CartItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Price = i.Product.Price,
                        Quantity = i.Quantity
                    }).ToList()
                });

        }

        public Result RemoveItem(string userId, int cartitemid)
        {
            var cart = GetByUserId(userId);
            if (!cart.IsSuccess)
            {
                return cart;
            }

            var existingItem = cart.Data.Items.FirstOrDefault(o => o.Id == cartitemid);///الحته ديه غالبا غلط لان المفروض انا اتاكد من ال id بتاع ال Cartitem مش ال product لما اجي امسح بسمح بال cartitemid مش المنتج
            if (existingItem is null)
            {
                return Result.Fail("The Cart Item isn't Exsit");
            }

            repositoryCart.Remove(existingItem);
            repositoryCart.Save();
            return Result.Success();

        }

        public Result UpdateQuantity(string userId, UpdateCartItemDto dto)
        {
            if (dto.Quantity <= 0)
                return Result.Fail("The Product Quantity Invalid");
            var cart = GetByUserId(userId);
            if (!cart.IsSuccess)
            {
                return cart;
            }

            var existingItem = cart.Data.Items.FirstOrDefault(o => o.Id == dto.cartitemid);
            if (existingItem is null)
            {
                return Result.Fail("The Cart Item isn't Exsit");
            }
            existingItem.Quantity = dto.Quantity;
            repositoryCart.Save();
            return Result.Success();

        }
    }
}

