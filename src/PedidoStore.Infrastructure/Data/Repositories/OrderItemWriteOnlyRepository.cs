using Microsoft.EntityFrameworkCore;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Entities.OrderAggregate;
using PedidoStore.Domain.Repositories;
using PedidoStore.Infrastructure.Data.Repositories.Common;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoStore.Infrastructure.Data.Repositories
{
    internal class OrderItemWriteOnlyRepository(WriteDbContext dbContext, IUnitOfWork unitOfWork)
    : BaseWriteOnlyRepository<OrderItem, Guid>(dbContext), IOrderItemWriteOnlyRepository
    {
        public async Task RemoveRange(Order order)
        {
            var existingItems = DbContext.OrderItems.AsNoTracking()
        .Where(i => i.OrderId == order.Id)
        .ToList();

            var missingRows = existingItems
                .Where(dbItem => !order.OrderItems.Any(inputItem => dbItem.Id == inputItem.Id))
                .ToList();

            foreach (var item in missingRows)
            {
                item.IsDeleted = true;
                DbContext.OrderItems.Update(item);
            }
            await DbContext.SaveChangesAsync();

       

        }

        public async Task UpdateByOrder(Order order)
        {

            foreach (var orderItem in order.OrderItems.ToList())
            {
                if (await DbContext.OrderItems.AnyAsync(x => x.Id == orderItem.Id))
                {
                    dbContext.Entry(orderItem).State = EntityState.Detached;
                    DbContext.Update(orderItem);
                    await DbContext.SaveChangesAsync();
                }
            }
        }

        public class OrderItemComparer : IEqualityComparer<OrderItem>
        {
            public bool Equals(OrderItem? x, OrderItem? y)
            {
                return x != null && y != null && x.Id == y.Id;
            }

            public int GetHashCode(OrderItem obj)
            {
                return obj.Id.GetHashCode();
            }
        }

    }
    public class OrderItemComparer : IEqualityComparer<OrderItem>
    {
        public bool Equals(OrderItem? x, OrderItem? y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id; // ou compare outras propriedades se necessário
        }

        public int GetHashCode(OrderItem obj)
        {
            return obj.Id.GetHashCode(); // use a mesma propriedade que usou no Equals
        }
    }
}
