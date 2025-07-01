

using MongoDB.Bson.Serialization;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Data.Mapping
{
    public class ProductMap : IReadDbMapping
    {
        public void Configure()
        {
            BsonClassMap.TryRegisterClassMap<ProductQueryModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);

                classMap.MapMember(order => order.Id)
                    .SetIsRequired(true);


                classMap.MapMember(customer => customer.OrderItems).SetElementName("order_items");
            });
        }
    }
}
