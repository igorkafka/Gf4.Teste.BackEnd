using MongoDB.Driver;
using PedidoStore.Query.Abstractions;

namespace PedidoStore.Query.Data.Repositories
{
    internal abstract class BaseReadOnlyRepository<TQueryModel, Tkey>(IReadDbContext context) : IReadOnlyRepository<TQueryModel, Tkey>
      where TQueryModel : IQueryModel<Tkey>
      where Tkey : IEquatable<Tkey>
    {
        protected readonly IMongoCollection<TQueryModel> Collection = context.GetCollection<TQueryModel>();

        /// <summary>
        /// Gets a query model by its id.
        /// </summary>
        /// <param name="id">The id of the query model.</param>
        /// <returns>The query model.</returns>
        public async Task<TQueryModel> GetByIdAsync(Tkey id)
        {
            using var asyncCursor = await Collection.FindAsync(queryModel => queryModel.Id.Equals(id));
            return await asyncCursor.FirstOrDefaultAsync();
        }
    }
}
