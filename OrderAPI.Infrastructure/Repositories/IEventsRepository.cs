using System;
using System.Threading.Tasks;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.Infrastructure.Repositores
{
    public interface IEventsRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        Task AppendAsync(TA aggregateRoot);
        Task<TA> RehydrateAsync(TKey key);
    }
}