using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using PedidoStore.Application.Orders.Commands;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Entities.OrderAggregate;
using PedidoStore.Domain.Repositories;


namespace PedidoStore.Application.Orders.CreateCustomerCommandHandler
{
    public class UpdateOrderCommandHandler
    (IValidator<UpdateOrderCommand> validator,
    IUnitOfWork unitOfWork,
    IOrderWriteOnlyRepository repository,
     IOrderItemWriteOnlyRepository orderItemWriteOnlyRepository,
    ICustomerWriteOnlyRepository repositoryCustomer,
    IProductWriteOnlyRepository repositoryProduct) : IRequestHandler<UpdateOrderCommand, Result>
{
    public async Task<Result> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await repositoryCustomer.GetFirst();

            // Validating the request.
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Returns the result with validation errors.
                return Result.Invalid(validationResult.AsErrors());
            }

            // Getting the customer from the database.
            var order = await repository.GetByIdAsync(request.Id);
            order.Customer = customer;
            if (order == null)
                return Result.NotFound($"No customer found by Id: {request.Id}");

            (bool flowControl, Result value) = await BuildOrderItem(request, order);
            if (!flowControl)
            {
                return value;
            }
            order.ChangeStatus(request.Status);

            if (!ValidateOrder(order)) return Result.Invalid(new ValidationError("Invalid Order"));


            repository.Update(order);

            // Saving the changes to the database and firing events.
            await unitOfWork.SaveChangesAsync();

            await orderItemWriteOnlyRepository.RemoveRangeByOrder(order);
            // Updating the entity in the repository.
            await orderItemWriteOnlyRepository.UpdateByOrder(order);

            return Result.SuccessWithMessage("Updated successfully!");
        }

        private async Task<(bool flowControl, Result value)> BuildOrderItem(UpdateOrderCommand request, Order order)
        {
            foreach (var orderItemRequest in request.OrderItems)
            {
                Product product = await repositoryProduct.GetByIdAsync(orderItemRequest.ProductId);
                if (orderItemRequest.Id == null)
                {
                    orderItemRequest.Id = Guid.NewGuid();
                }
                OrderItem orderItem = new OrderItem((Guid)orderItemRequest.Id, order.Id, product.Id, orderItemRequest.UnitPrice, orderItemRequest.Quantity);

                var orderItemResult = order.AddItem(orderItem);
                if (!orderItemResult.IsSuccess)
                    return (flowControl: false, value: Result.Invalid(orderItemResult.ValidationErrors));
            }

            return (flowControl: true, value: null);
        }

        private bool ValidateOrder(Order order)
        {
            order.ChangeTotalAmount();

            return true;
        }
    }
}
