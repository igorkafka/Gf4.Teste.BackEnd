
namespace PedidoStore.Core.SharedKernel
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }
}
