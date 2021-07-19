using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.Domain.Commands
{
    public class SyncOrderFromCosmos
    {
        public class Command : IRequest
        {
            public Command()
            {
                // TODO: implement this with correct properties
            }

            public Guid Id { get; set; }
            public string BranchName { get; set; }
            public string CustomerId { get; set; }
            public string CustomerName { get; set; }
            public Decimal TotalValue { get; set; }
            public Decimal DiscountValue { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IEventsService<Order, Guid> _service;

            public Handler(IEventsService<Order, Guid> service)
            {
                _service = service;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = await _service.RehydrateAsync(request.Id);
                if (null == order)
                    throw new ArgumentOutOfRangeException(nameof(Command.Id), "invalid customer id");
                
                throw new NotImplementedException();
            }
        }
    }
}