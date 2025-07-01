
using MongoDB.Bson.Serialization;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Data.Mapping
{
    public class CustomerMap : IReadDbMapping
    {
        public void Configure()
        {
            BsonClassMap.TryRegisterClassMap<CustomerQueryModel>(classMap =>
            {
                classMap.AutoMap();
                classMap.SetIgnoreExtraElements(true);

                classMap.MapMember(order => order.Id)
                    .SetIsRequired(true);
            });
        }
    }
}
