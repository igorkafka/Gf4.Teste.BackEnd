
using Ardalis.Result;
using MediatR;
using PedidoStore.Application.Orders.Response;
using PedidoStore.Domain.Entities.OrderAggregate;

namespace PedidoStore.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest<Result<CreatedOrderResponse>>
    {
        public Guid? CustomerId { get; set; }
        public EStatus Status { get; set; }
        public virtual ICollection<CreatedOrderItemCommand> OrderItems { get; set; }

    }
}
