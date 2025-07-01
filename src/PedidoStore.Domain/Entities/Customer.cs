using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities.OrderAggregate;
using PedidoStore.Domain.ValueObjects;

namespace PedidoStore.Domain.Entities
{
    public class Customer : BaseEntity
    {

        public new Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Email Email { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
