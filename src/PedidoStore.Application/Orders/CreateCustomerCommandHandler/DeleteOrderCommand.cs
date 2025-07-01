
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using PedidoStore.Application.Orders.Commands;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Repositories;

namespace PedidoStore.Application.Orders.CreateCustomerCommandHandler
{
    public class DeleteOrderCommandHandler(
    IValidator<DeleteOrderCommand> validator,
    IOrderWriteOnlyRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderCommand, Result>
    {
        public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            // Validating the request.
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Returns the result with validation errors.
                return Result.Invalid(validationResult.AsErrors());
            }

            var order = await repository.GetByIdAsync(request.Id);
            if (order == null)
                return Result.NotFound($"No Order found by Id: {request.Id}");

            // Marking the entity as deleted, the CustomerDeletedEvent will be added.
            order.Delete();

            // Removing the entity from the repository.
            repository.Remove(order);

            // Saving the changes to the database and triggering the events.
            await unitOfWork.SaveChangesAsync();

            // Returning the success message.
            return Result.SuccessWithMessage("Successfully removed!");
        }
    }
}