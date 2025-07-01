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
            request.CustomerId = customer.Id;
           
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
        foreach (var orderItemRequest in request.OrderItems)
        {
            Product product = await repositoryProduct.GetByIdAsync(orderItemRequest.ProductId);
             OrderItem orderItem = new OrderItem(orderItemRequest.Id, order.Id, product.Id, orderItemRequest.UnitPrice, orderItemRequest.Quantity);

             var orderItemResult = order.AddItem(orderItem);
            if (!orderItemResult.IsSuccess)
                return Result.Invalid(orderItemResult.ValidationErrors);
        }
        order.ChangeStatus(request.Status);

        if (!ValidateOrder(order)) return Result.Invalid(new ValidationError("Invalid Order"));


         repository.Update(order);

        // Saving the changes to the database and firing events.
        await unitOfWork.SaveChangesAsync();

        await orderItemWriteOnlyRepository.RemoveRange(order);
        // Updating the entity in the repository.
        await orderItemWriteOnlyRepository.UpdateByOrder(order);
        
        await unitOfWork.SaveChangesAsync();

        return Result.SuccessWithMessage("Updated successfully!");
    }
        private bool ValidateOrder(Order order)
        {
            order.ChangeTotalAmount();

            return true;
        }
    }
}
