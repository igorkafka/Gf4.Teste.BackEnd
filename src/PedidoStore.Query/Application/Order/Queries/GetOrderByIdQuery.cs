using Ardalis.Result;
using MediatR;
using PedidoStore.Query.QueriesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoStore.Query.Application.Order.Queries
{
    public class GetOrderByIdQuery(Guid id) : IRequest<Result<OrderQueryModel>>
    {
        public Guid Id { get; } = id;
    }
}
