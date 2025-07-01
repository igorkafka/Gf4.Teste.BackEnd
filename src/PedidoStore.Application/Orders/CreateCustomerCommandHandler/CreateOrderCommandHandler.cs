    
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using PedidoStore.Application.Orders.Commands;
using PedidoStore.Application.Orders.Response;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Entities.OrderAggregate;
using PedidoStore.Domain.Entities.OrderAggregate.Events;
using PedidoStore.Domain.Factories;
using PedidoStore.Domain.Repositories;

namespace PedidoStore.Application.Orders.CreateCustomerCommandHandler
{
    public class CreateOrderCommandHandler(
     IOrderWriteOnlyRepository repository,
          ICustomerWriteOnlyRepository repositoryCustomer,
                    IProductWriteOnlyRepository repositoryProduct,

    IValidator<CreateOrderCommand> validator,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Result<CreatedOrderResponse>>
    {
        public async Task<Result<CreatedOrderResponse>> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await repositoryCustomer.GetFirst();   
            // Validating the request.
            
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Return the result with validation errors.
                return Result<CreatedOrderResponse>.Invalid(validationResult.AsErrors());
            }

            // Instantiating the Email value object.


            // Creating an instance of the customer entity.
            // When instantiated, the "CustomerCreatedEvent" will be created.
            var order = OrderFactory.CreateWithoutResult(
                customer.Id, request.Status);


            foreach (var orderItemRequest in request.OrderItems)
            {
                Product product = await repositoryProduct.GetByIdAsync(orderItemRequest.ProductId);
                OrderItem orderItem = new OrderItem(order.Id, product.Id, orderItemRequest.UnitPrice, orderItemRequest.Quantity);
                var orderItemResult = order.AddItem(orderItem);
                if (!orderItemResult.IsSuccess)
                    return Result.Invalid(orderItemResult.ValidationErrors);
            }

            if (!ValidateOrder(order)) return Result.Invalid(new ValidationError("Invalid Order"));

            // Adding the entity to the repository.
            repository.Add(order);

            // Saving changes to the database and triggering events.
            await unitOfWork.SaveChangesAsync();
            // Returning the ID.
            return Result<CreatedOrderResponse>.Created(
                new CreatedOrderResponse(order.Id), location: $"/api/customers/{order.Id}");
        }
        private bool ValidateOrder(Order order)
        {
            order.CompleteOrder();

            return true;
        }
    }
}
