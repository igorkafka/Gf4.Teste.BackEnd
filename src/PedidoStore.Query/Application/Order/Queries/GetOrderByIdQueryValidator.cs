using FluentValidation;

namespace PedidoStore.Query.Application.Order.Queries
{
    public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdQueryValidator()
        {
            RuleFor(command => command.Id)
                .NotEmpty();
        }
    }
}
