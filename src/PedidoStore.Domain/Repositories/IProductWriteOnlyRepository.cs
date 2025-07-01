

using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities;

namespace PedidoStore.Domain.Repositories
{
    public interface IProductWriteOnlyRepository:IWriteOnlyRepository<Product, Guid>
    {
    }
}
