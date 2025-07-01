

using Ardalis.Result;
using MediatR;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Query.Application.Order.Queries;
using PedidoStore.Query.Data.Repositories.Abstractions;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Application.Order.Handlers
{
    public class GetAllOrderQueryHandler(IOrderReadOnlyRepository repository, ICacheService cacheService)
        : IRequestHandler<GetAllOrderQuery, Result<IEnumerable<OrderQueryModel>>>
    {
        private const string CacheKey = nameof(GetAllOrderQuery);

        public async Task<Result<IEnumerable<OrderQueryModel>>> Handle(
              GetAllOrderQuery request,
              CancellationToken cancellationToken)
        {
            // This method will either return the cached data associated with the CacheKey
            // or create it by calling the GetAllAsync method.
            return Result<IEnumerable<OrderQueryModel>>.Success(
                await repository.GetAllAsync());
        }
    }
}
