using ECommerce.Domain.Domain_Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    internal class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.UserId).IsRequired();
            builder.HasOne<ApplicationUser>().WithOne().HasForeignKey<Cart>(P => P.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
