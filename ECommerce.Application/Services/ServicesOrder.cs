using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
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
        public Cart? GetCartByUserId(string userId) // helper
        {
            var cart = repositoryCart.GetByUserId(userId);
            if (cart is null)
            {
                return null;
            }
            return cart;
        }
        public Order? GetEntityById(int orderId) // helper
        {
            var order = repositoryOrder.GetById(orderId);
            if (order is null)
            {
                return null;
            }
            return order;
        }
        public bool Checkout(string userId, CheckOutDto dto)
        {
            var cart = repositoryCart.GetByUserId(userId);
            if (cart is null||!cart.Items.Any())
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(dto.ShippingAddress))
            {
                return false;
            }

            var products = new Dictionary<int, Product>();
            foreach (var item in cart.Items)
            {
                var product = repositoryProduct.GetById(item.ProductId);

                if (product is null)
                {
                    return false;
                }
                if (item.Quantity > product.StockQuantity)
                {
                    return false;
                }
                products.Add(product.Id,product);

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
            return true;

        }

        public OrderDto? GetOrderById(int orderId)
        {
            var order = repositoryOrder.GetById(orderId);

            return new OrderDto()
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

            };

        }

        public List<OrderDto> GetOrders(string userId)
        {
            var order = repositoryOrder.GetByUserId(userId);
            if (order is null)
            {
                return null;
            }

            return order.Select(i => new OrderDto()
            {
                IdOrder = i.Id,
                ProductCount = i.Items.Count,
                TotalPrice = i.TotalPrice,
                CreatedAt = i.CreatedAt,
            }).ToList();

        }
    }
}
