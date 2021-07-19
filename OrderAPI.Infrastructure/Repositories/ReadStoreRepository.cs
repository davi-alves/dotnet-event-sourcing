using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.Infrastructure.Repositores
{
    public class ReadStoreRepository<TA, TKey> : IReadStoreRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        private readonly IDynamoDBContext _context;

        public ReadStoreRepository(IAmazonDynamoDB amazonDynamoDb)
        {
            _context = new DynamoDBContext(amazonDynamoDb);
        }
        
        
        public async Task<TA> GetAsync(TKey key)
        {
            var entity = await _context.LoadAsync<TA>(key.ToString());
           
            return entity;
        }

        public async Task<TA> SaveAsync(TA aggregateRoot)
        {
            await _context.SaveAsync(aggregateRoot);

            return aggregateRoot;
        }
    }
}