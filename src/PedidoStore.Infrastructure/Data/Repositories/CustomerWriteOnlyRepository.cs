using Microsoft.EntityFrameworkCore;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Repositories;
using PedidoStore.Infrastructure.Data.Repositories.Common;

namespace PedidoStore.Infrastructure.Data.Repositories
{
    internal class CustomerWriteOnlyRepository(WriteDbContext dbContext)
    : BaseWriteOnlyRepository<Customer, Guid>(dbContext), ICustomerWriteOnlyRepository
    {

        public async Task<Customer> GetFirst()
        {
           return dbContext.Customers.AsNoTracking().FirstOrDefault();
        }
    }
}
