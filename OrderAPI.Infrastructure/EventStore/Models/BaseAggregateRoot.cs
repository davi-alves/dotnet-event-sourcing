using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OrderApi.Infrastructure.EventStore
{
    public abstract class BaseAggregateRoot<TA, TKey> : BaseEntity<TKey>, IAggregateRoot<TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        private readonly Queue<IDomainEvent<TKey>> _events = new Queue<IDomainEvent<TKey>>();
        
        protected BaseAggregateRoot() { }
        
        protected BaseAggregateRoot(TKey id) : base(id)
        {
        }

        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();

        [JsonIgnore]
        public long Version { get; private set; }

        public void ClearEvents()
        {
            _events.Clear();
        }

        protected void AddEvent(IDomainEvent<TKey> @event)
        {
            _events.Enqueue(@event);
           
            this.Apply(@event);

            this.Version++;
        }

        protected abstract void Apply(IDomainEvent<TKey> @event);

        #region Factory

        private static readonly ConstructorInfo Ctor;

        static BaseAggregateRoot()
        {
            var aggregateType = typeof(TA);
            Ctor = aggregateType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null, new Type[0], new ParameterModifier[0]);
            if (null == Ctor)
                throw new InvalidOperationException($"Unable to find required private parameterless constructor for Aggregate of type '{aggregateType.Name}'");
        }

        public static TA Create(IEnumerable<IDomainEvent<TKey>> events)
        {
            if(null == events)
                throw new ArgumentNullException(nameof(events));

            var domainEvents = events.ToList(); 
            if(!domainEvents.Any())
                throw new ArgumentNullException(nameof(events));
            
            var result = (TA)Ctor.Invoke(new object[0]);
            if (result is BaseAggregateRoot<TA, TKey> baseAggregate) 
                foreach (var @event in domainEvents)
                    baseAggregate.AddEvent(@event);

            result.ClearEvents();

            return result;
        }

        #endregion Factory
    }
}