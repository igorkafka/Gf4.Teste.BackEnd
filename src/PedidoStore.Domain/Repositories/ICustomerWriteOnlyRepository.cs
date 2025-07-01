
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities;

namespace PedidoStore.Domain.Repositories
{
    public interface ICustomerWriteOnlyRepository :IWriteOnlyRepository<Customer, Guid>
    {
        Task<Customer> GetFirst();
    }
}
