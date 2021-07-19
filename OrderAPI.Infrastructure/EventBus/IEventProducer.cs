using System.Threading.Tasks;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.OrderAPI.Infrastructure.EventBus
{
    public interface IEventProducer<in TA, in TKey>
        where TA : IAggregateRoot<TKey>
    {
        Task DispatchAsync(TA aggregateRoot);
    }
}