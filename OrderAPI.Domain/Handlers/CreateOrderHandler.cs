using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Domain.Commands;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.OrderAPI.Domain.Handlers
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand>
    {
       
        private readonly IEventsService<Order, Guid> _service;

        public CreateOrderHandler(IEventsService<Order, Guid> service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
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