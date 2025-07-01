

using Microsoft.EntityFrameworkCore;
using PedidoStore.Domain.Entities.OrderAggregate;
using PedidoStore.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using PedidoStore.Domain.Repositories;

namespace PedidoStore.Infrastructure.Data.Repositories;
internal class OrderWriteOnlyRepository(WriteDbContext dbContext)
    : BaseWriteOnlyRepository<Order, Guid>(dbContext), IOrderWriteOnlyRepository
{
    public override void Update(Order entity)
    {
        dbContext.Entry(entity).State = EntityState.Detached;

        // Only mark the Status property as modified
        dbContext.Entry(entity).Property(x => x.Status).IsModified = true;
        dbContext.Entry(entity).Property(x => x.TotalAmount).IsModified = true;
        DbContext.Update(entity);
        DbContext.SaveChanges();
    }
}

