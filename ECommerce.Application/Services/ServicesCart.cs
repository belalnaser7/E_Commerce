using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Services
{
    public class ServicesCart : IServicesCart
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;

        public ServicesCart(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
        }
        public async Task<Result<Cart?>> GetByUserIdAsync(string userId) // helper
        {
            string key = $"Cart:{userId}";
            var cart = cacheService.Get<Cart>(key);
            if (cart is null)
            {
                var cart1 = await unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart1 is null)
                {
                    return Result<Cart?>.Fail("The Cart isn't Exsit", ErrorType.NotFound);
                }
                cacheService.Set(key, TimeSpan.FromHours(2), cart1);
                cart = cart1;

            }

            return Result<Cart?>.Success(cart);
        }

        public async Task<Result> AddToCartAsync(string userId, AddToCartDto dto)
        {
            

            var product = await unitOfWork.Products.GetByStatusIdAsync(dto.ProductId,ProductStatus.Approved);
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

                await unitOfWork.Carts.AddAsync(cart);
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
            cacheService.Remove($"CartItems:{userId}");
            cacheService.Remove($"Cart:{userId}");
            return Result.Success();
        }

        public async Task<Result> ClearCartAsync(string userId)
        {
            var cart = await GetByUserIdAsync(userId);
            if (!cart.IsSuccess)
            {
                return cart;
            }

            cart.Data.Items.Clear();

            await unitOfWork.SaveAsync();
            cacheService.Remove($"CartItems:{userId}");
            cacheService.Remove($"Cart:{userId}");
            return Result.Success();
        }

        public async Task<Result<CartDto?>> GetCartAsync(string userId)
        {
            string key = $"CartItems:{userId}";
            var cartDto = cacheService.Get<CartDto?>(key);
            if (cartDto is null)
            {
                var cart = await GetByUserIdAsync(userId);
                if (!cart.IsSuccess)
                {
                    return Result<CartDto?>.Fail("The Cart isn't Exsit", ErrorType.NotFound);
                }
                var listCartItems = cart.Data.Items.Select(i => new CartItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Price = i.Product.Price,
                    Quantity = i.Quantity
                }).ToList();
                cartDto = new CartDto
                {
                    Items = listCartItems
                };
                cacheService.Set(key, TimeSpan.FromHours(2), cartDto);
               
            }

            return Result<CartDto?>.Success(cartDto);

            //return Result<CartDto?>.Success(
            //    new CartDto
            //    {
            //        Items = cart.Data.Items.Select(i => new CartItemDto
            //        {
            //            ProductId = i.ProductId,
            //            ProductName = i.Product.Name,
            //            Price = i.Product.Price,
            //            Quantity = i.Quantity
            //        }).ToList()
            //    });

        }

        public async Task<Result> RemoveItemAsync(string userId, int cartitemid)
        {
            var cart = await GetByUserIdAsync(userId);
            if (!cart.IsSuccess)
            {
                return cart;
            }

            var existingItem = cart.Data.Items.FirstOrDefault(o => o.Id == cartitemid);///الحته ديه غالبا غلط لان المفروض انا اتاكد من ال id بتاع ال Cartitem مش ال product لما اجي امسح بسمح بال cartitemid مش المنتج
            if (existingItem is null)
            {
                return Result.Fail("The Cart Item isn't Exsit");
            }

            unitOfWork.Carts.Remove(existingItem);
            await unitOfWork.SaveAsync();
            cacheService.Remove($"CartItems:{userId}");
            cacheService.Remove($"Cart:{userId}");
            return Result.Success();

        }

        public async Task<Result> UpdateQuantityAsync(string userId, UpdateCartItemDto dto)
        {
            //if (dto.Quantity <= 0)
            //    return Result.Fail("The Product Quantity Invalid");
            var cart = await GetByUserIdAsync(userId);
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
            cacheService.Remove($"CartItems:{userId}");
            cacheService.Remove($"Cart:{userId}");
            return Result.Success();

        }
    }
}

