
using System.Linq.Expressions;

namespace PedidoStore.Query.Abstractions
{
    public interface ISynchronizeDb : IDisposable
    {
        Task UpsertAsync<TQueryModel>(TQueryModel queryModel, Expression<Func<TQueryModel, bool>> upsertFilter)
            where TQueryModel : IQueryModel;
        Task InsertAsync<TQueryModel>(TQueryModel queryModel)
      where TQueryModel : IQueryModel;

        Task DeleteAsync<TQueryModel>(Expression<Func<TQueryModel, bool>> deleteFilter)
            where TQueryModel : IQueryModel;
    }
}
