using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Result_pattern;
using ECommerce.Domain.Domain_Models;

namespace ECommerce.Application.Services
{
    public class ServicesOrder : IServicesOrder
    {
        private readonly IRepositoryCart repositoryCart;
        private readonly IRepositoryOrder repositoryOrder;
        private readonly IRepositoryProduct repositoryProduct;
        public ServicesOrder(IRepositoryCart repositoryCart, IRepositoryOrder repositoryOrder, IRepositoryProduct repositoryProduct)
        {
            this.repositoryCart = repositoryCart;
            this.repositoryOrder = repositoryOrder;
            this.repositoryProduct = repositoryProduct;
        }
        public Result<Cart?> GetCartByUserId(string userId) // helper
        {
            var cart = repositoryCart.GetByUserId(userId);
            if (cart is null)
            {
                return Result<Cart?>.Fail("The Cart isn't Exsit", ErrorType.NotFound);
            }
            return Result<Cart?>.Success(cart);
        }
        public Result<Order?> GetEntityById(int orderId) // helper
        {
            var order = repositoryOrder.GetById(orderId);
            if (order is null)
            {
                return Result<Order?>.Fail("The Order isn't Exsit", ErrorType.NotFound);
            }
            return Result<Order?>.Success(order);
        }
        public Result Checkout(string userId, CheckOutDto dto)
        {
            var cart = repositoryCart.GetByUserId(userId);
            if (cart is null || !cart.Items.Any())
            {
                return Result.Fail("The cart isn't Exsit or Empty", ErrorType.NotFound);
            }
            if (string.IsNullOrWhiteSpace(dto.ShippingAddress))
            {
                return Result.Fail("The Address isn't Exsit", ErrorType.NotFound);
            }

            var products = new Dictionary<int, Product>();
            foreach (var item in cart.Items)
            {
                var product = repositoryProduct.GetById(item.ProductId);

                if (product is null)
                {
                    return Result.Fail("The Product  isn't Exsit", ErrorType.NotFound);
                }
                if (item.Quantity > product.StockQuantity)
                {
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
            repositoryOrder.Add(order);
            cart.Items.Clear();
            repositoryOrder.Save();
            return Result.Success();

        }

        public Result<OrderDto?> GetOrderById(int orderId)
        {
            var order = repositoryOrder.GetById(orderId);
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

        public Result<List<OrderDto>> GetOrders(string userId)
        {
            var order = repositoryOrder.GetByUserId(userId);


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
