using MongoDB.Driver;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.Data.Repositories.Abstractions;
using PedidoStore.Query.QueriesModel;


namespace PedidoStore.Query.Data.Repositories
{
    internal class ProductReadOnlyRepository(IReadDbContext readDbContext): BaseReadOnlyRepository<ProductQueryModel, Guid>(readDbContext), IProductReadOnlyRepository
    {
        public async Task<IEnumerable<ProductQueryModel>> GetAllAsync()
        {
            var sort = Builders<ProductQueryModel>.Sort
                .Descending(customer => customer.Name);

            var findOptions = new FindOptions<ProductQueryModel>
            {
                Sort = sort
            };

            using var asyncCursor = await Collection.FindAsync(Builders<ProductQueryModel>.Filter.Empty, findOptions);
            return await asyncCursor.ToListAsync();
        }
    }
}
