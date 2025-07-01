using Microsoft.EntityFrameworkCore;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Entities.OrderAggregate;
using PedidoStore.Infrastructure.Mappings;
using System.Linq.Expressions;


namespace PedidoStore.Infrastructure.Data
{
    public class WriteDbContext(DbContextOptions<WriteDbContext> dbOptions)
       : BaseDbContext<WriteDbContext>(dbOptions)
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply query filter to all entities implementing ISoftDeletable
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(GetIsDeletedFilter(entityType.ClrType));
                }
            }

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfigurationConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ProductMap());
            base.OnModelCreating(modelBuilder);
        }
        private static LambdaExpression GetIsDeletedFilter(Type type)
        {
            var parameter = Expression.Parameter(type, "e");
            var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
            var constant = Expression.Constant(false);
            var equals = Expression.Equal(property, constant);
            return Expression.Lambda(equals, parameter);
        }
    }
}
