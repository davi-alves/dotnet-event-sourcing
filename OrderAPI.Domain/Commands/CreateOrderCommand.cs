using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.Domain.Commands
{
    public class CreateOrderCommand : IRequest
    {
        public CreateOrderCommand(Guid? transactionId, Guid customerId, decimal value, List<OrderItem> items, bool confirmed)
        {
            Id = transactionId ?? Guid.NewGuid();
            CustomerId = customerId;
            Value = value;
            Items = items;
            Confirmed = confirmed;
        }

        public Guid Id { get; }
        public Guid CustomerId { get; }
        public decimal Value { get; }
        public List<OrderItem> Items { get; }
        public bool Confirmed { get; }

    }
}