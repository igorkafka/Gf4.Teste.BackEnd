
using AutoMapper.Configuration.Annotations;
using MongoDB.Bson.Serialization.Attributes;
using PedidoStore.Query.Abstractions;
using System.Text.Json.Serialization;

namespace PedidoStore.Query.QueriesModel
{
    public class OrderQueryModel : IQueryModel<Guid>
    {
        public Guid CustomerId { get; set; }

        public string OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public virtual ICollection<OrderItemQueryModel> OrderItems { get; set; }
        [Ignore]
        [BsonIgnore]
        public ProductQueryModel Product { get; set; }
        [Ignore]
        [BsonIgnore]
        public CustomerQueryModel Customer { get; set; }

        public Guid Id { get; set; }
        public OrderQueryModel(Guid id, Guid customerId, decimal totalAmount, string orderDate, string status, ICollection<OrderItemQueryModel> orderItemQueryModels)
        {
            Id = id;
            CustomerId = customerId;
            TotalAmount = totalAmount;
            OrderDate = orderDate;
            Status = status;
            OrderItems = orderItemQueryModels;
        }
        public OrderQueryModel()
        {

        }
    }
}
