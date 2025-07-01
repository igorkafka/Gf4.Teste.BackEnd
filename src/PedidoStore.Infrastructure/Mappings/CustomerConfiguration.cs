

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.ValueObjects;
using PedidoStore.Infrastructure.Extensions;

namespace PedidoStore.Infrastructure.Mappings
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
            .ConfigureBaseEntity();

            builder
                .Property(c => c.Name)
                .IsRequired() // NOT NULL
                .HasMaxLength(200);


            builder.HasData(new Customer() { Id = Guid.Parse("4a5f1c24-b68a-46f3-b1a3-1b2f0ea3dc08"), Name = "user", Phone = "(69) 99297-1717"});

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.Address)
                     .HasDefaultValue("user@gmail.com")
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.AddressMaxLength})");
            });

        }
    }
}
