using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Services
{
    public class ServicesOrder : IServicesOrder
    {
      
        private readonly IUnitOfWork unitOfWork;

        public ServicesOrder(IUnitOfWork unitOfWork)
        {
           
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<Cart?>> GetCartByUserIdAsync(string userId) // helper
        {
            var cart =await unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart is null)
            {
                return Result<Cart?>.Fail("The Cart isn't Exsit", ErrorType.NotFound);
            }
            return Result<Cart?>.Success(cart);
        }
        public async Task<Result<Order?>> GetEntityByIdAsync(int orderId) // helper
        {
            var order =await unitOfWork.Orders.GetByIdAsync(orderId);
            if (order is null)
            {
                return Result<Order?>.Fail("The Order isn't Exsit", ErrorType.NotFound);
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
            var order = await unitOfWork.Orders.GetByIdAsync(orderId);
            if (order is null)
            {
                return Result<OrderDto?>.Fail(
                    "The Order isn't Exsit",
                    ErrorType.NotFound);
            }

            return Result<OrderDto?>.Success(
                new OrderDto()
                {
                    IdOrder = orderId,
                    ProductCount = order.Items.Count(),
                    TotalPrice = order.TotalPrice,
                    CreatedAt = order.CreatedAt,
                    Items = order.Items.Select(i => new OrderItemDto
                    {
                        ProductName = i.Product.Name,
                        UnitPrice = i.UnitPrice,
                        Quantity = i.Quantity
                    }).ToList()

                });

        }

        public async Task<Result<List<OrderDto>>> GetOrdersAsync(string userId)
        {
            var order = await unitOfWork.Orders.GetByUserIdAsync(userId);


            return Result<List<OrderDto>>.Success(
                order.Select(i => new OrderDto()
                {
                    IdOrder = i.Id,
                    ProductCount = i.Items.Count,
                    TotalPrice = i.TotalPrice,
                    CreatedAt = i.CreatedAt,
                }).ToList());

        }
    }
}
