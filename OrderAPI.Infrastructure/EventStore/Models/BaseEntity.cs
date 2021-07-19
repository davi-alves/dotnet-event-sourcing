namespace OrderApi.Infrastructure.EventStore
{
    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        protected BaseEntity() { }

        protected BaseEntity(TKey id) => Id = id;

        public TKey Id { get; protected set; }

        // TODO: add compare methods
    }
}