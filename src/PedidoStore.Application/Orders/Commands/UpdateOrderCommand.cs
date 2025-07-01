
using Ardalis.Result;
using MediatR;
using PedidoStore.Domain.Entities.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace PedidoStore.Application.Orders.Commands
{
    public class UpdateOrderCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public EStatus Status { get; set; }
        public virtual ICollection<CreatedOrderItemCommand> OrderItems { get; set; }
    }
}
