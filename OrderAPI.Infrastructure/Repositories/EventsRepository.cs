using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Infrastructure.Core;
using OrderApi.Infrastructure.EventStore;
using OrderApi.Infrastructure.Persistence;

namespace OrderApi.Infrastructure.Repositores
{
    public class EventsRepository<TA, TKey> : IEventsRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        private readonly EntityEventContext _context;
        private readonly IEventSerializer _serializer;

        public EventsRepository(EntityEventContext context, IEventSerializer serializer)
        {
            _context = context;
            _serializer = serializer;
        }

        public async Task AppendAsync(TA aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            foreach (var @event in aggregateRoot.Events)
            {
                var entityEvent = Map(@event);
                _context.EntityEvent.Add(entityEvent);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<TA> RehydrateAsync(TKey key)
        {
            // TODO: in the future getting the stream of events might be better for performance
            var entries = await _context.EntityEvent.Where(e => e.AggregateId == key.ToString())
                .OrderBy(x => x.AggregateVersion)
                .ToListAsync();
            if (!entries.Any())
                return null;

            var events = entries.Select(Map).ToList();
            var result = BaseAggregateRoot<TA, TKey>.Create(events);

            return result;
        }

        private EntityEvent Map(IDomainEvent<TKey> @event)
        {
            var aggregateId = @event.AggregateId.ToString();
            var data = _serializer.Serialize(@event);
            var eventType = @event.GetType().Name;

            var eventPayload = EntityEvent.Create(
                aggregateId,
                @event.AggregateVersion,
                eventType,
                data,
                @event.Timestamp
            );

            return eventPayload;
        }

        private IDomainEvent<TKey> Map(EntityEvent @event)
        {
            return _serializer.Deserialize<TKey>(@event.EventType, @event.EventData);
        }
    }
}