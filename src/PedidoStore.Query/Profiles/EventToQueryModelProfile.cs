using AutoMapper;
using PedidoStore.Domain.Entities;
using PedidoStore.Domain.Entities.OrderAggregate.Events;
using PedidoStore.Query.QueriesModel;


namespace PedidoStore.Query.Profiles
{
    public class EventToQueryModelProfile : Profile
    {
        public EventToQueryModelProfile()
        {
            CreateMap<OrderCreatedEvent, OrderQueryModel>(MemberList.Destination)
                .ConstructUsing(@event => CreateOrderQueryModel(@event))
               .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => DomainListOrderItemToQuery(src.OrderItems)))
                      .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<OrderUpdatedEvent, OrderQueryModel>(MemberList.Destination)
                .ConstructUsing(@event => CreateOrderQueryModel(@event)).
             ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => DomainListOrderItemToQuery(src.OrderItems)))
                    .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<OrderDeletedEvent, OrderQueryModel>(MemberList.Destination)
                .ConstructUsing(@event => CreateOrderQueryModel(@event))
             .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => DomainListOrderItemToQuery(src.OrderItems)))
              .ForMember(dest => dest.Customer, opt => opt.Ignore())
               .ForMember(dest => dest.Product, opt => opt.Ignore());



        }

        public override string ProfileName => nameof(EventToQueryModelProfile);
        private static CustomerQueryModel DomainCustomerToQuery(Customer customer) => new CustomerQueryModel(customer.Id, customer.Name, customer.Phone, customer.Email.Address);
        private static ProductQueryModel DomainProductToQuery(Product product) => new ProductQueryModel(product.Id, product.Name, product.Price);
        private static IEnumerable<OrderItemQueryModel> DomainListOrderItemToQuery(IEnumerable<OrderItemBaseEvent> orderItems) => orderItems.Select(x => new OrderItemQueryModel(x.Id, x.OrderId, x.ProductId,  x.UnitPrice, x.TotalPrice, x.Quantity )).ToList();

        private static OrderQueryModel CreateOrderQueryModel<TEvent>(TEvent @event) where TEvent : OrderBaseEvent =>
            new(@event.Id, @event.CustomerId,@event.TotalAmount,@event.OrderDate.ToString(), @event.Status.ToString(), DomainListOrderItemToQuery(@event.OrderItems).ToList());
    }
}
