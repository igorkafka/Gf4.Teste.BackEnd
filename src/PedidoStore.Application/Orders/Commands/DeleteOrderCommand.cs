
using Ardalis.Result;
using MediatR;

namespace PedidoStore.Application.Orders.Commands
{
    public class DeleteOrderCommand(Guid id) : IRequest<Result>
    {
        public Guid Id { get; } = id;
    }
}
