using Ardalis.Result;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities.OrderAggregate.Events;
using System.Security.Cryptography.X509Certificates;

namespace PedidoStore.Domain.Entities.OrderAggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        private bool _isDeleted;
        public virtual Guid Id { get; set; } = Guid.NewGuid();

        public Guid CustomerId { get; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public EStatus Status { get; private set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public Order(Guid customerId, EStatus status) : base()
        {
            CustomerId = customerId;
            OrderDate = DateTime.Now;
            OrderItems = new List<OrderItem>();
            Status = status;
        }
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public Result<OrderItem> AddItem(OrderItem orderItem)
        {


            orderItem.CalculateOrderAmount();
            if (orderItem.TotalPrice <= 0)
                return Result.Invalid(new ValidationError("Quantity must be greater than zero."));
            OrderItems.Add(orderItem);
            return Result.Success(orderItem);
        }
        public void ChangeTotalAmount()
        {
            CalculeTotalAmount();
            AddDomainEvent(new OrderUpdatedEvent(Id, TotalAmount, Status, CustomerId, OrderDate, SetOrderItemEvents()));
        }
        private void CalculeTotalAmount()
        {
            TotalAmount = OrderItems.Sum(or => or.TotalPrice);
        }
        public void CompleteOrder()
        {
            CalculeTotalAmount();
            AddDomainEvent(new OrderCreatedEvent(Id, TotalAmount, Status, CustomerId, OrderDate, SetOrderItemEvents()));
            /// add logic
        }
        private ICollection<OrderItemBaseEvent> SetOrderItemEvents()
        {
            IList<OrderItemBaseEvent> orderItemBaseEvents = new List<OrderItemBaseEvent>();
            foreach (var orderItem in OrderItems)
            {
                orderItemBaseEvents.Add(new OrderItemBaseEvent(orderItem.Id,
                    Id, orderItem.ProductId, orderItem.UnitPrice, orderItem.TotalPrice, orderItem.Quantity));
            }
            return orderItemBaseEvents;
        }
        public void AuthorizeOrder() => Status = EStatus.Authorized;

        public void MarkAsPaid() => Status = EStatus.Paid;

        public void DeclineOrder() => Status = EStatus.Declined;

        public void MarkAsDelivered() => Status = EStatus.Delivered;

        public void CancelOrder() => Status = EStatus.Canceled;
        public void Delete()
        {
            if (_isDeleted) return;

            _isDeleted = true;
            AddDomainEvent(new OrderDeletedEvent(Id, TotalAmount, Status, CustomerId, OrderDate, new List<OrderItemBaseEvent>()));
        }
        public Result ChangeStatus(EStatus newStatus)
        {
            if (Status == newStatus)
                return Result.Invalid(new ValidationError("The order is already in this status."));

            switch (newStatus)
            {
                case EStatus.Authorized:
                    AuthorizeOrder();
                    break;

                case EStatus.Paid:
                    MarkAsPaid();
                    break;

                case EStatus.Declined:
                    DeclineOrder();
                    break;

                case EStatus.Delivered:
                    MarkAsDelivered();
                    break;

                case EStatus.Canceled:
                    CancelOrder();
                    break;

                default:
                    return Result.Invalid(new ValidationError("Invalid status."));
            }

            return Result.Success();
        }
    }
}
