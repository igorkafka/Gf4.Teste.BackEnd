using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PedidoStore.Core.Extensions;
using PedidoStore.Core.SharedKernel;
using PedidoStore.Domain.Entities.OrderAggregate.Events;
using PedidoStore.Query.Abstractions;
using PedidoStore.Query.Application.Order.Queries;
using PedidoStore.Query.QueriesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PedidoStore.Query.EventHandlers
{
    public class OrderEventHandler(
      IMapper mapper,
      ISynchronizeDb synchronizeDb,
      ICacheService cacheService,
      ILogger<OrderEventHandler> logger) :
      INotificationHandler<OrderCreatedEvent>,
      INotificationHandler<OrderUpdatedEvent>,
      INotificationHandler<OrderDeletedEvent>
    {
        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            LogEvent(notification);

            var orderQueryModel = mapper.Map<OrderQueryModel>(notification);
            await synchronizeDb.InsertAsync(orderQueryModel);
            await ClearCacheAsync(notification);
        }

        public async Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            LogEvent(notification);

            var customerQueryModel = mapper.Map<OrderQueryModel>(notification);
            await synchronizeDb.UpsertAsync(customerQueryModel, filter => filter.Id == customerQueryModel.Id);
            await ClearCacheAsync(notification);
        }

        public async Task Handle(OrderDeletedEvent notification, CancellationToken cancellationToken)
        {
            LogEvent(notification);

            var customerQueryModel = mapper.Map<OrderQueryModel>(notification);
            await synchronizeDb.DeleteAsync<CustomerQueryModel>(filter => filter.Id == notification.Id);
            await ClearCacheAsync(notification);
        }

        private async Task ClearCacheAsync(OrderBaseEvent @event)
        {
            var cacheKeys = new[] { nameof(GetAllOrderQuery), $"{nameof(GetOrderByIdQuery)}_{@event.Id}" };
            await cacheService.RemoveAsync(cacheKeys);
        }

        private void LogEvent<TEvent>(TEvent @event) where TEvent : OrderBaseEvent =>
            logger.LogInformation("----- Triggering the event {EventName}, model: {EventModel}", typeof(TEvent).Name, @event.ToJson());
    }
}
