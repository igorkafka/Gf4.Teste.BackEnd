

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PedidoStore.Domain.Entities;
using PedidoStore.Infrastructure.Extensions;
using System.Reflection.Emit;

namespace PedidoStore.Infrastructure.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
            .ConfigureBaseEntity();

            builder
            .Property(p => p.Price)
            .HasPrecision(18, 2);

            builder.HasData(new Product() { Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Name = "Laptop", Price = 1500 },
                new Product() { Id = Guid.Parse("b8a0f735-c46b-4e63-84de-88a8ab5cbe02"), Name = "Nintendo Switch", Price = 2250 }, 
                new Product() { Id = Guid.Parse("d1c3a881-2a4d-4e9f-9214-8cbf6ff88e44"), Name = "PC Gamer", Price = 2250 });
         }
    }
}
