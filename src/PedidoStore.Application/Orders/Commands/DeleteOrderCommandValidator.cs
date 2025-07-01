
using FluentValidation;

namespace PedidoStore.Application.Orders.Commands
{
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty();
        }
    }
}
