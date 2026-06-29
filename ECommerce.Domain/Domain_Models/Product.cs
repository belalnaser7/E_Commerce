using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Domain_Models
{
    public enum ProductStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        //Hidden = 3,
        //OutOfStock = 4
    }
    public class Product
    {
        public int Id { get; set; }
        public string SellerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ProductStatus Status { get; set; } = ProductStatus.Pending;
    }
}
