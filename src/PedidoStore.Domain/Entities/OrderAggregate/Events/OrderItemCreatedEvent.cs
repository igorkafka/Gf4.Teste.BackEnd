
namespace PedidoStore.Domain.Entities.OrderAggregate.Events
{
    public class OrderItemCreatedEvent(Guid id, Guid orderId, Guid productId, decimal unitPrice, decimal totalPrice, int quantity) 
        : OrderItemBaseEvent(id, orderId, productId, unitPrice, totalPrice, quantity) 
    {

    }
}
