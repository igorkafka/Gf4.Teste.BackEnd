using Ardalis.Result;
using MediatR;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Query.Application.Order.Queries;
using PedidoStore.Query.Data.Repositories.Abstractions;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Application.Order.Handlers
{
    public class GetAllProductQueryHandler(IProductReadOnlyRepository repository, ICacheService cacheService)
         : IRequestHandler<GetAllProductQuery, Result<IEnumerable<ProductQueryModel>>>
    {
        private const string CacheKey = nameof(GetAllProductQuery);

        public async Task<Result<IEnumerable<ProductQueryModel>>> Handle(
              GetAllProductQuery request,
              CancellationToken cancellationToken)
        {
            // This method will either return the cached data associated with the CacheKey
            // or create it by calling the GetAllAsync method.
            return Result<IEnumerable<ProductQueryModel>>.Success(await repository.GetAllAsync());
        }
    }
}
