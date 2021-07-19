using System;

namespace OrderApi.Infrastructure.Persistence
{
    public class EntityEvent
    {
        public Guid Id { get; set; }
        public string AggregateId { get; set; }
        public long AggregateVersion { get; set; }
        public string EventType { get; set; }
        public byte[] EventData { get; set; }
        public DateTime Timestamp { get; set; }
        
        public static EntityEvent Create(string aggregateId, long aggregateVersion, string type, byte[] data, DateTime timestamp)
        {
            if (data == null) 
                throw new ArgumentNullException(nameof(data));
            
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(type));

            return new EntityEvent()
            {
                Id = Guid.NewGuid(),
                AggregateId = aggregateId,
                AggregateVersion = aggregateVersion,
                EventType = type,
                EventData = data,
                Timestamp = timestamp
            };
        }
    }
}