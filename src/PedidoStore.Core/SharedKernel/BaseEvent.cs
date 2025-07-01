

using MediatR;

namespace PedidoStore.Core.SharedKernel
{
    public class BaseEvent : INotification
    {
        public string MessageType { get; protected init; }

        public Guid AggregateId { get; protected init; }

        public DateTime OccurredOn { get; private init; } = DateTime.Now;
    }
}
