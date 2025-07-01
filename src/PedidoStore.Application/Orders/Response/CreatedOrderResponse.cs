using PedidoStore.Core.SharedKernel;

namespace PedidoStore.Application.Orders.Response
{
    public class CreatedOrderResponse(Guid id) : IResponse
    {
        public Guid Id { get; } = id;
    }
}
