using System;
using System.Threading.Tasks;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;

namespace OrderApi.Infrastructure.EventStore
{
    public interface IEventsService<TA, TKey> 
        where TA : class, IAggregateRoot<TKey>
    {
        Task PersistAsync(TA aggregateRoot);
        Task<TA> RehydrateAsync(TKey key);
    }
}