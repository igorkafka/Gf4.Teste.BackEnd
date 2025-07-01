
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities.OrderAggregate.Events;

namespace PedidoStore.Application.Orders.Response
{
    public class CreatedOrderItemResponse(Guid id)
    {
        public Guid Id { get; } = id;
    }
}
