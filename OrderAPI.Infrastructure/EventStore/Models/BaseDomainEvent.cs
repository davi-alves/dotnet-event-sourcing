using System;

namespace OrderApi.Infrastructure.EventStore
{
    public class BaseDomainEvent<TA, TKey> : IDomainEvent<TKey>
        where TA : IAggregateRoot<TKey>
    {
        protected BaseDomainEvent() { }
        protected BaseDomainEvent(TA aggregateRoot)
        {
            if(aggregateRoot is null)
                throw new ArgumentNullException(nameof(aggregateRoot));

            AggregateVersion = aggregateRoot.Version;
            AggregateId = aggregateRoot.Id;
            Timestamp = DateTime.UtcNow;
        }

        public long AggregateVersion { get; private set; }
        public TKey AggregateId { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}