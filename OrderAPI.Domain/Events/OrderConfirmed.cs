using System;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Infrastructure.EventStore;

namespace OrderAPI.Domain.Events
{
    public class OrderConfirmed : BaseDomainEvent<Order, Guid>
    {
        private OrderConfirmed() { }

        public OrderConfirmed(Order order) : base(order)
        {
            Status = OrderStatus.Pending.Name;
        }

        public string Status { get; private set; }
    }
}