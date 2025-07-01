

using MediatR;
using PedidoStore.Core.SharedKernel;
using System.Collections;

namespace PedidoStore.Domain.Entities.OrderAggregate.Events
{
    public abstract class OrderBaseEvent : BaseEvent
    {
        protected OrderBaseEvent(
       Guid id,
       decimal totalAmount,
       EStatus status,
       Guid customerId,
       DateTime orderDate, ICollection<OrderItemBaseEvent> orderItemBases)
        {
            Id = id;
            AggregateId = id;
            CustomerId = customerId;
            TotalAmount = totalAmount;
            OrderDate = orderDate;
            Status = status;
            OrderItems = orderItemBases;
        }

        public Guid Id { get; private init; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public EStatus Status { get; set; }
        public ICollection<OrderItemBaseEvent> OrderItems { get; set; }
    }
}
