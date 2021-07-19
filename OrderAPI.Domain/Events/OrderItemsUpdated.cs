using System;
using System.Collections.Generic;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Infrastructure.EventStore;

namespace OrderAPI.Domain.Events
{
    public class OrderItemsUpdated : BaseDomainEvent<Order, Guid>
    {
        private OrderItemsUpdated()
        {
        }

        public OrderItemsUpdated(Order order, List<OrderItem> items) : base(order)
        {
            Items = items;
        }

        public List<OrderItem> Items { get; private set; }
    }
}