using ECommerce.Domain.Domain_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
            builder.Property(p => p.ImageUrl).HasMaxLength(100); 
            builder.Property(p => p.Description).IsRequired().HasMaxLength(300);
            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(p => p.SellerId).IsRequired();
            builder.Property(p => p.StockQuantity).IsRequired();
            builder.Property(p => p.Status).HasConversion<int>();
            builder.HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(o => o.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<ApplicationUser>()
                .WithMany()
               .HasForeignKey(o => o.SellerId)
               .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
