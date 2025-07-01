
using FluentValidation;

namespace PedidoStore.Application.Orders.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(command => command.OrderItems)
             .Must(CreateOrderCommandValidator.ValidateOrderItems).WithMessage("Items of the order must have more than one item!");
        }
        private static bool ValidateOrderItems(ICollection<CreatedOrderItemCommand> OrderItems)
        {
            return OrderItems.Count != 0;
        }
    }
}
