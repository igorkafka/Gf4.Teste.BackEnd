
using PedidoStore.Core.SharedKernel;

namespace PedidoStore.Domain.Entities.OrderAggregate.Events
{
    public class OrderItemBaseEvent : BaseEvent
    {
        public OrderItemBaseEvent(Guid id, Guid orderId, Guid productId, decimal unitPrice, decimal totalPrice, int quantity)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            UnitPrice = unitPrice;
            TotalPrice = totalPrice;
            Quantity = quantity;    
        }
        public Guid Id { get; private init; }

        public Guid OrderId { get; private init; }
        public Guid ProductId { get; set; }

        public decimal UnitPrice { get; private init; }
        public decimal TotalPrice { get; private init; }
        public int Quantity { get; private init; }
    }
}
