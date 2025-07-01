using MongoDB.Driver;
using PedidoStore.Domain.Entities;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.Data.Repositories.Abstractions;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Data.Repositories
{
    internal class OrderReadOnlyRepository(IReadDbContext readDbContext
)
        : BaseReadOnlyRepository<OrderQueryModel, Guid>(readDbContext), IOrderReadOnlyRepository
    {
        public async Task<IEnumerable<OrderQueryModel>> GetAllAsync()
        {
            var sort = Builders<OrderQueryModel>.Sort
                .Descending(customer => customer.OrderDate);

            var findOptions = new FindOptions<OrderQueryModel>
            {
                Sort = sort
            };

            var asyncCursor = await Collection.FindAsync(Builders<OrderQueryModel>.Filter.Empty, findOptions);
            var orders = await asyncCursor.ToListAsync();
            foreach (var order in orders)
            {
                order.Customer = readDbContext.GetCollection<CustomerQueryModel>().Find(x => x.Id == order.CustomerId).First();
                foreach (var ordemItem in order.OrderItems)
                {
                    ordemItem.Product = readDbContext.GetCollection<ProductQueryModel>().Find(x => x.Id == ordemItem.ProductId).First();
                }

            }

            return orders;
        }
    }
}
