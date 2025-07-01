using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities.OrderAggregate;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.Application.Order.Queries;
using PedidoStore.Query.Data.Repositories.Abstractions;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Application.Order.Handlers
{
    internal class GetOrderByIdQueryHandler(
    IValidator<GetOrderByIdQuery> validator,
    IOrderReadOnlyRepository repository,
    IProductReadOnlyRepository productReadOnlyRepository,
    ICustomerReadOnlyRepository customerReadOnlyRepository,
    ICacheService cacheService) : IRequestHandler<GetOrderByIdQuery, Result<OrderQueryModel>>
    {
        public async Task<Result<OrderQueryModel>> Handle(
            GetOrderByIdQuery request,
            CancellationToken cancellationToken)
        {
            // Validating the request.
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                // Returns the result with validation errors.
                return Result<OrderQueryModel>.Invalid(validationResult.AsErrors());
            }

            // Creating a cache key using the query name and the customer ID.
            var cacheKey = $"{nameof(GetOrderByIdQuery)}_{request.Id}";

            // Getting the customer from the cache service. If not found, fetches it from the repository.
            // The customer will be stored in the cache service for future queries.
            var order = await repository.GetByIdAsync(request.Id);

            order.Customer = await customerReadOnlyRepository.GetByIdAsync(order.CustomerId);
            foreach (var ordemItem in order.OrderItems)
            {
                ordemItem.Product = await productReadOnlyRepository.GetByIdAsync(ordemItem.ProductId);    
            }

            // If the customer is null, returns a result indicating that no customer was found.
            // Otherwise, returns a successful result with the customer.
            return order == null
                ? Result<OrderQueryModel>.NotFound($"No customer found by Id: {request.Id}")
                : Result<OrderQueryModel>.Success(order);
        }
    }
}
