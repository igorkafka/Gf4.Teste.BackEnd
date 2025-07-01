

namespace PedidoStore.Domain.Entities.OrderAggregate.Events
{
    public class OrderUpdatedEvent(
       Guid id,
       decimal totalAmount,
       EStatus status,
       Guid customerId,
       DateTime orderDate, ICollection<OrderItemBaseEvent> orderItemBases) : OrderBaseEvent(id, totalAmount, status, customerId, orderDate, orderItemBases);
}
