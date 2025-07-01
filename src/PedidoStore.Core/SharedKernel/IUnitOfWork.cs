
namespace PedidoStore.Core.SharedKernel;
public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync();
}

