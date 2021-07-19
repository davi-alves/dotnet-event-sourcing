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
    public class CreateOrder
    {
        public class Command : IRequest
        {
            public Command(Guid? transactionId, Guid customerId, Decimal value, List<OrderItem> items, bool confirmed)
            {
                Id = transactionId ?? Guid.NewGuid();
                CustomerId = customerId;
                Value = value;
                Items = items;
                Confirmed = confirmed;
            }

            public Guid Id { get; }
            public Guid CustomerId { get; }
            public Decimal Value { get; }
            public List<OrderItem> Items { get; }
            public bool Confirmed { get; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IEventsService<Order, Guid> _service;

            public Handler(IEventsService<Order, Guid> service)
            {
                _service = service;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                var order = new Order(
                    command.Id,
                    command.CustomerId,
                    command.Value
                );
                if (command.Items != null && command.Items.Any())
                    order.SetOrderItems(command.Items);

                if (command.Confirmed)
                    order.ConfirmOrder();

                await _service.PersistAsync(order);
                
                return Unit.Value;
            }
        }
    }
}