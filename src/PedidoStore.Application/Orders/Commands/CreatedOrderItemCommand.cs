

using Ardalis.Result;
using MediatR;
using PedidoStore.Application.Orders.Response;

namespace PedidoStore.Application.Orders.Commands
{
    public class CreatedOrderItemCommand : IRequest<Result<CreatedOrderItemResponse>>
    {

        public Guid? Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
