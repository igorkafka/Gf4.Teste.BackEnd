
using Ardalis.Result;
using MediatR;
using PedidoStore.Query.QueriesModel;

namespace PedidoStore.Query.Application.Order.Queries;

public class GetAllOrderQuery : IRequest<Result<IEnumerable<OrderQueryModel>>>;
