using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities.OrderAggregate;

namespace PedidoStore.Domain.Repositories
{
    public interface IOrderWriteOnlyRepository : IWriteOnlyRepository<Order, Guid>
    {
    }
}
