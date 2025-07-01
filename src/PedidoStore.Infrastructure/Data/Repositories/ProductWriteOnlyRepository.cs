
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Entities.OrderAggregate;
using PedidoStore.Domain.Repositories;
using PedidoStore.Infrastructure.Data.Repositories.Common;

namespace PedidoStore.Infrastructure.Data.Repositories
{
    internal class ProductWriteOnlyRepository(WriteDbContext dbContext)
    : BaseWriteOnlyRepository<Product, Guid>(dbContext), IProductWriteOnlyRepository
    {
    }
}
