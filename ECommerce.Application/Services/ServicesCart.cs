using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Services
{
    public class ServicesCart : IServicesCart
    {
        private readonly IRepositoryCart repositoryCart;
        private readonly IRepositoryProduct repositoryProduct;
        private readonly IUnitOfWork unitOfWork;

        public ServicesCart(IRepositoryCart repositoryCart, IRepositoryProduct repositoryProduct,IUnitOfWork unitOfWork)
        {
            this.repositoryCart = repositoryCart;
            this.repositoryProduct = repositoryProduct;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<Cart?>> GetByUserIdAsync(string userId) // helper
        {
            var cart = await repositoryCart.GetByUserIdAsync(userId);
            if (cart is null)
            {
                return Result<Cart?>.Fail("The Cart isn't Exsit", ErrorType.NotFound);
            }
            return Result<Cart?>.Success(cart);
        }

        public async Task<Result> AddToCartAsync(string userId, AddToCartDto dto)
        {
            //if (dto.Quantity <= 0)
            //    return Result.Fail("The Product Quantity Invalid");

            var product = await repositoryProduct.GetByIdAsync(dto.ProductId);
            if (product is null)
            {
                return Result.Fail("The Product isn't Exsit");
            }
            var cartResult = await GetByUserIdAsync(userId);
            Cart cart;

            if (!cartResult.IsSuccess)
            {
               
                cart = new Cart()
                {
                    UserId = userId
                };

               await repositoryCart.AddAsync(cart);
            }
            else
            {
                cart = cartResult.Data;
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
            await unitOfWork.SaveAsync();

            return Result.Success();
        }

        public async Task<Result> ClearCartAsync(string userId)
        {
            var cart =await GetByUserIdAsync(userId);
            if (!cart.IsSuccess)
            {
                return cart;
            }

            cart.Data.Items.Clear();

            await unitOfWork.SaveAsync();
            return Result.Success();
        }

        public async Task<Result<CartDto?>> GetCartAsync(string userId)
        {
            var cart =await GetByUserIdAsync(userId);
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

        public async Task<Result> RemoveItemAsync(string userId, int cartitemid)
        {
            var cart =await GetByUserIdAsync(userId);
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
            await unitOfWork.SaveAsync();
            return Result.Success();

        }

        public async Task<Result> UpdateQuantityAsync(string userId, UpdateCartItemDto dto)
        {
            if (dto.Quantity <= 0)
                return Result.Fail("The Product Quantity Invalid");
            var cart =await GetByUserIdAsync(userId);
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
            await unitOfWork.SaveAsync();
            return Result.Success();

        }
    }
}

