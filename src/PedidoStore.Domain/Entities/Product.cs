
using PedidoStore.Core.SharedKernel;

namespace PedidoStore.Domain.Entities
{
    public class Product : BaseEntity
    {
        public new Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        public Product()
        {

        }
    }
}
