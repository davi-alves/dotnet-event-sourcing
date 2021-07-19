using System;
using System.Threading.Tasks;
using OrderApi.Infrastructure.Repositores;

namespace OrderApi.Infrastructure.EventStore
{
    public class EventsService<TA, TKey> : IEventsService<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        private readonly IEventsRepository<TA, TKey> _writeRepository;
        private readonly IReadStoreRepository<TA, TKey> _readRepository;

        public EventsService(IEventsRepository<TA, TKey> writeRepository, IReadStoreRepository<TA, TKey> readRepository)
        {
            _writeRepository = writeRepository;
            _readRepository = readRepository;
        }

        public async Task PersistAsync(TA aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            // TODO: event producer
            await _writeRepository.AppendAsync(aggregateRoot);
            await _readRepository.SaveAsync(aggregateRoot);
            
            aggregateRoot.ClearEvents();
        }

        public Task<TA> RehydrateAsync(TKey key)
        {
            return _writeRepository.RehydrateAsync(key);
        }
    }
}