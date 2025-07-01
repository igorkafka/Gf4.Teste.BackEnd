
using FluentValidation;

namespace PedidoStore.Application.Orders.Commands
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty();

            RuleFor(command => command.CustomerId)
                         .NotEmpty();

        }
    }
}
