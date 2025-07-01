
using Ardalis.Result;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Entities.OrderAggregate;

namespace PedidoStore.Domain.Factories
{
    public class OrderItemFactory
    {
        public static Result<OrderItem> Create(Guid orderId, Guid  productId, decimal unitPrice, int quantity)
                            {
                                return Result<OrderItem>.Success(new OrderItem(orderId, productId, unitPrice, quantity));
                            }

        public static OrderItem CreateWithoutResult(Guid orderId,Guid productId, decimal unitPrice, int quantity)
            => new OrderItem(orderId, productId, unitPrice, quantity);
    }
}
