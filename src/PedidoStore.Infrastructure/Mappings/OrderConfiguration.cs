

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PedidoStore.Domain.Entities.OrderAggregate;
using PedidoStore.Infrastructure.Extensions;

namespace PedidoStore.Infrastructure.Mappings
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
         .ConfigureBaseEntity();

            builder
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2);

            builder
                      .HasOne(o => o.Customer)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(o => o.CustomerId);
        }
    }
}
