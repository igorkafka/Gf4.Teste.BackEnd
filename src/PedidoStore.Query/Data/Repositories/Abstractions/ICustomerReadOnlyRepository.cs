using PedidoStore.Query.Abstractions;
using PedidoStore.Query.QueriesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoStore.Query.Data.Repositories.Abstractions
{
    public interface ICustomerReadOnlyRepository : IReadOnlyRepository<CustomerQueryModel, Guid>
    {
    }
}
