

using MongoDB.Driver;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.Data.Repositories.Abstractions;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Data.Repositories
{
    internal class CustomerReadOnlyRepository(IReadDbContext readDbContext) : BaseReadOnlyRepository<CustomerQueryModel, Guid>(readDbContext), ICustomerReadOnlyRepository
    {
    }
}
