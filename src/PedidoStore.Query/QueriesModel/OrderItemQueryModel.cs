

using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PedidoStore.Query.QueriesModel
{
    public class OrderItemQueryModel 
    {
        public OrderItemQueryModel()
        {

        }
        public OrderItemQueryModel(Guid id, Guid orderId, Guid productId, decimal unitPrice, decimal totalPrice, int quantity)
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
        [BsonElement("order")]
        public OrderQueryModel Order { get; set; }
        public Guid ProductId { get; private init; }
        [BsonElement("product")]
        public ProductQueryModel Product { get; set; }
        public decimal UnitPrice { get; private init; }
        public decimal TotalPrice { get; private init; }
        public int Quantity { get; set; }
    }
}
