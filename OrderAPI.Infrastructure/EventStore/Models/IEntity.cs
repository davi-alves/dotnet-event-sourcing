namespace OrderApi.Infrastructure.EventStore
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}