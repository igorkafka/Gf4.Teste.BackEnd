

using PedidoStore.Query.Abstractions;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Data.Repositories.Abstractions
{
    public interface IProductReadOnlyRepository : IReadOnlyRepository<ProductQueryModel, Guid>
    {
        Task<IEnumerable<ProductQueryModel>> GetAllAsync();
    }
}
