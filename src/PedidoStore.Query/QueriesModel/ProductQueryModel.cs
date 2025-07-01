
using PedidoStore.Query.Abstractions;

namespace PedidoStore.Query.QueriesModel
{
    public class ProductQueryModel : IQueryModel<Guid>
    {
        public ProductQueryModel()
        {

        }
        public ProductQueryModel(Guid id,string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
        public Guid Id { get; private init; }
        public string Name { get; private init; }
        public decimal Price { get; private init; }
        public virtual ICollection<OrderItemQueryModel> OrderItems { get; set; }
    }
}
