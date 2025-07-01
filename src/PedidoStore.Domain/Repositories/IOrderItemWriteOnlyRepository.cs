

using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Entities.OrderAggregate;

namespace PedidoStore.Domain.Repositories
{
    public interface IOrderItemWriteOnlyRepository : IWriteOnlyRepository<OrderItem, Guid>
    {
        Task RemoveRange(Order order);
        Task UpdateByOrder(Order order);
    }
}
