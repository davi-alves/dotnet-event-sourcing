using System.Threading.Tasks;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.Infrastructure.Repositores
{
    public interface IReadStoreRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        Task<TA> GetAsync(TKey key);
        Task<TA> SaveAsync(TA aggregateRoot);
    }
}