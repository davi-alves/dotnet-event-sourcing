using System;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Infrastructure.EventStore;

namespace OrderAPI.Domain.Events
{
    public class OrderCreated : BaseDomainEvent<Order, Guid>
    {
        private OrderCreated()
        {
        }

        public OrderCreated(Order order) : base(order)
        {
            Id = order.Id;
            CustomerId = order.CustomerId;
            Value = order.Value;
        }

        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public Decimal Value { get; private set; }
    }
}