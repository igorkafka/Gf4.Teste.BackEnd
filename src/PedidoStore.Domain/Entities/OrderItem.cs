

using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities.OrderAggregate;

namespace PedidoStore.Domain.Entities
{
    public class OrderItem : BaseEntity, ISoftDeletable
    {
        protected OrderItem()
        {

        }
        public OrderItem(Guid orderId, Guid productId, decimal unitPrice, int quantity) : base()
        {
            ProductId = productId;
            OrderId = orderId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
        public OrderItem(Guid id, Guid orderId, Guid productId, decimal unitPrice, int quantity) : base()
        {
            Id = id;
            ProductId = productId;
            OrderId = orderId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public Guid? OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get;  set; }

        public void CalculateOrderAmount()
        {
            TotalPrice = UnitPrice * Quantity;
        }
        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
