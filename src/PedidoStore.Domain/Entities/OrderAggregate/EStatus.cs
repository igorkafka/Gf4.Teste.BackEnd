

namespace PedidoStore.Domain.Entities.OrderAggregate
{
    public enum EStatus
    {
        Authorized = 1,
        Paid = 2,
        Declined = 3,
        Delivered = 4,
        Canceled = 5
    }
}
