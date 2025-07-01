
namespace PedidoStore.Query.Abstractions
{
    public interface IReadOnlyRepository<TQueryModel, in TKey>
     where TQueryModel : IQueryModel<TKey>
     where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the query model by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the query model.</param>
        /// <returns>The task representing the asynchronous operation, returning the query model.</returns>
        Task<TQueryModel> GetByIdAsync(TKey id);
    }
}
