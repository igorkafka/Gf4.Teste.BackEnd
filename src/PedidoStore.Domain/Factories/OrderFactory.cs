using Ardalis.Result;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Entities.OrderAggregate;

namespace PedidoStore.Domain.Factories
{
    public static class OrderFactory
    {
        public static Result<Order> Create(
            Guid customerId, EStatus status)
        {
            return  Result<Order>.Success(new Order(customerId, status));
        }

        public static Order CreateWithoutResult(Guid customerId, EStatus status)
            => new Order(customerId, status);
    }
}
