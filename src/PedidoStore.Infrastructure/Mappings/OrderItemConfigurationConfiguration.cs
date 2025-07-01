

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PedidoStore.Domain.Entities;
using PedidoStore.Infrastructure.Extensions;

namespace PedidoStore.Infrastructure.Mappings
{
    public class OrderItemConfigurationConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder
               .ConfigureBaseEntity();

            builder
            .Property(p => p.TotalPrice)
            .HasPrecision(18, 2);

            builder
          .Property(p => p.UnitPrice)
          .HasPrecision(18, 2);

            builder
     .Property(p => p.IsDeleted)
     .HasDefaultValue(false);

            builder
           .HasOne(orderItem => orderItem.Order)
               .WithMany(order => order.OrderItems)
               .HasForeignKey(orderItem => orderItem.OrderId).OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(orderItem => orderItem.Product)
                .WithMany(product => product.OrderItems)
                .HasForeignKey(orderItem => orderItem.ProductId);
        }
    }
}
