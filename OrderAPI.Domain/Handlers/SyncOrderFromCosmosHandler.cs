using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Domain.Commands;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.OrderAPI.Domain.Handlers
{
    public class SyncOrderFromCosmosHandler: IRequestHandler<SyncOrderFromCosmosCommand>
    {
        private readonly IEventsService<Order, Guid> _service;

        public SyncOrderFromCosmosHandler(IEventsService<Order, Guid> service)
        {
            _service = service;
        }

        public async Task<Unit> Handle(SyncOrderFromCosmosCommand request, CancellationToken cancellationToken)
        {
            var order = await _service.RehydrateAsync(request.Id);
            if (null == order)
                throw new ArgumentOutOfRangeException(nameof(SyncOrderFromCosmosCommand.Id), "invalid customer id");
                
            throw new NotImplementedException();
        }
    }
}