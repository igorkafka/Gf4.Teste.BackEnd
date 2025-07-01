using PedidoStore.Query.Abstractions;
using PedidoStore.Query.QueriesModel;


namespace PedidoStore.Query.Data.Repositories.Abstractions
{
    public interface IOrderReadOnlyRepository : IReadOnlyRepository<OrderQueryModel, Guid>
    {
        Task<IEnumerable<OrderQueryModel>> GetAllAsync();
    }
}
