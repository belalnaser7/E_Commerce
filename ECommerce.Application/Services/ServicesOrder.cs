using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Services
{
    public class ServicesOrder : IServicesOrder
    {
      
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;

        public ServicesOrder(IUnitOfWork unitOfWork,ICacheService cacheService)
        {
           
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
        }
        public async Task<Result<Cart?>> GetCartByUserIdAsync(string userId) // helper
        {
            string key = $"Cart:{userId}";
            var cart = cacheService.Get<Cart?>(key);
            if (cart is null)
            {
                var cart1 = await unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart1 is null)
                {
                    return Result<Cart?>.Fail("The Cart isn't Exsit", ErrorType.NotFound);
                }
                cacheService.Set(key, TimeSpan.FromMinutes(20), cart1);
                cart = cart1;
            }
            return Result<Cart?>.Success(cart);
        }
        public async Task<Result<Order?>> GetEntityByIdAsync(int orderId) // helper
        {
            string key = $"order:{orderId}";
            var order = cacheService.Get<Order?>(key);
            if (order is null )
            {
                var order1 = await unitOfWork.Orders.GetByIdAsync(orderId);
                if (order1 is null)
                {
                    return Result<Order?>.Fail("The Order isn't Exsit", ErrorType.NotFound);
                }
                cacheService.Set(key, TimeSpan.FromMinutes(20), order1);
                order = order1;
            }
            
            return Result<Order?>.Success(order);
        }
        public async Task<Result> CheckoutAsync(string userId, CheckOutDto dto)
        {
            await unitOfWork.BeginTransactionAsync();
            try
            {
                var cart = await unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart is null || cart.Items is null || !cart.Items.Any())
                {
                    await unitOfWork.RollbackAsync();
                    return Result.Fail("The cart isn't Exsit or Empty", ErrorType.NotFound);
                }
                //if (string.IsNullOrWhiteSpace(dto.ShippingAddress))
                //{
                //    return Result.Fail("The Address isn't Exsit", ErrorType.NotFound);
                //}
                var products = new Dictionary<int, Product>();
                foreach (var item in cart.Items)
                {
                    var product = await unitOfWork.Products.GetByIdAsync(item.ProductId);

                    if (product is null)
                    {
                        await unitOfWork.RollbackAsync();
                        return Result.Fail("The Product  isn't Exsit", ErrorType.NotFound);
                    }
                    if (item.Quantity > product.StockQuantity)
                    {
                        await unitOfWork.RollbackAsync();
                        return Result.Fail("The Quantity is Ivalid");
                    }
                    products.Add(product.Id, product);
                }
                var order = new Order()
                {
                    UserId = userId,
                    ShippingAddress = dto.ShippingAddress,
                    Items = cart.Items.Select(i =>
                    {
                        var product = products[i.ProductId];
                        return (new OrderItem()
                        {
                            ProductId = i.ProductId,
                            Quantity = i.Quantity,
                            UnitPrice = product.Price
                        });
                    }).ToList()
                };
                order.TotalPrice = order.Items.Sum(i => i.UnitPrice * i.Quantity);
                foreach (var item in cart.Items)
                {
                    var product = products[item.ProductId];
                    product.StockQuantity -= item.Quantity;
                }
                await unitOfWork.Orders.AddAsync(order);
                cart.Items.Clear();
                await unitOfWork.SaveAsync();
                await unitOfWork.CommitAsync();
                cacheService.Remove($"orders:{userId}");
                cacheService.Remove($"Cart:{userId}");
                cacheService.Remove($"CartItems:{userId}");
                cacheService.Remove("Products");

                foreach (var item in order.Items)
                {
                    cacheService.Remove($"Product:{item.ProductId}");
                    cacheService.Remove($"productEntity:{item.ProductId}");
                    cacheService.Remove($"orderItems:{item.OrderId}");
                }
                return Result.Success();
            }
            catch 
            {
                await unitOfWork.RollbackAsync();
                return Result.Fail("Something went wrong");
            }
            
        }
        public async Task<Result<OrderDto?>> GetOrderByIdAsync(int orderId)
        {
            string key = $"orderItems:{orderId}";
            var orderDto = cacheService.Get<OrderDto?>(key);
            if (orderDto is null)
            {
                var order = await unitOfWork.Orders.GetByIdAsync(orderId);
                if (order is null)
                {
                    return Result<OrderDto?>.Fail(
                        "The Order isn't Exsit",
                        ErrorType.NotFound);
                }
                var orderItems = order.Items.Select(i => new OrderItemDto
                {
                    ProductName = i.Product.Name,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity
                }).ToList();
                orderDto = new OrderDto()
                {
                    IdOrder = orderId,
                    ProductCount = order.Items.Count(),
                    TotalPrice = order.TotalPrice,
                    CreatedAt = order.CreatedAt,
                    Items = orderItems
                };
                cacheService.Set(key, TimeSpan.FromMinutes(20), orderDto);
            }
            return Result<OrderDto?>.Success(orderDto);

            //return Result<OrderDto?>.Success(
            //    new OrderDto()
            //    {
            //        IdOrder = orderId,
            //        ProductCount = order.Items.Count(),
            //        TotalPrice = order.TotalPrice,
            //        CreatedAt = order.CreatedAt,
            //        Items = order.Items.Select(i => new OrderItemDto
            //        {
            //            ProductName = i.Product.Name,
            //            UnitPrice = i.UnitPrice,
            //            Quantity = i.Quantity
            //        }).ToList()

            //    });

        }

        public async Task<Result<List<OrderDto>>> GetOrdersAsync(string userId)
        {

            string key = $"orders:{userId}";
            var orderDto = cacheService.Get<List<OrderDto>>(key);
            if (orderDto is null)
            {
                var order = await unitOfWork.Orders.GetByUserIdAsync(userId);
                orderDto = order.Select(i => new OrderDto()
                {
                    IdOrder = i.Id,
                    ProductCount = i.Items.Count,
                    TotalPrice = i.TotalPrice,
                    CreatedAt = i.CreatedAt,
                }).ToList();
                cacheService.Set(key, TimeSpan.FromMinutes(20), orderDto);

            }
            return Result<List<OrderDto>>.Success(orderDto);


            //return Result<List<OrderDto>>.Success(
            //    order.Select(i => new OrderDto()
            //    {
            //        IdOrder = i.Id,
            //        ProductCount = i.Items.Count,
            //        TotalPrice = i.TotalPrice,
            //        CreatedAt = i.CreatedAt,
            //    }).ToList());

        }
    }
}
