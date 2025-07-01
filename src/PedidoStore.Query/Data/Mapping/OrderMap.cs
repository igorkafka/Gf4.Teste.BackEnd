
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Data.Mapping
{
    public class OrderMap : IReadDbMapping
    {
        public void Configure()
        {
            BsonClassMap.TryRegisterClassMap<OrderQueryModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);

                classMap.MapMember(order => order.Id)
                    .SetIsRequired(true);

                classMap.MapMember(order => order.TotalAmount)
                    .SetIsRequired(true);

                classMap.MapMember(order => order.OrderDate)
                    .SetIsRequired(true)
                    .SetSerializer(new DateTimeSerializer(true));

                classMap.MapMember(order => order.OrderItems).SetElementName("order_items");
            });
        }
    }
}
