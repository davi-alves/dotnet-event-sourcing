using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using OrderApi.Infrastructure.EventStore;
using OrderAPI.Domain.Events;
using OrderAPI.Infrastructure.Core;

namespace OrderAPI.Domain.AggregatesModel.OrderAggregates
{
    [DynamoDBTable("Order")]
    public class Order : BaseAggregateRoot<Order, Guid>
    {
        public Order() { }
        
        public Order(Guid id, Guid customerId, Decimal value) : base(id)
        {
            Id = id;
            CustomerId = customerId;
            Value = value;
            Status = OrderStatus.Draft.Name;
            
            AddEvent(new OrderCreated(this));
        }
        
        [DynamoDBHashKey]
        [DynamoDBProperty(typeof(DynamodbGuidToStringConverter))]
        public new Guid Id { get; set; }
        
        [DynamoDBProperty("customerId", typeof(DynamodbGuidToStringConverter))]
        public Guid CustomerId { get; set; }
        
        [DynamoDBProperty("value")]
        public decimal Value { get; set; }
        
        [DynamoDBProperty("status")]
        public string Status { get; set; }
        
        [DynamoDBProperty("items")]
        public List<OrderItem> Items { get; set; }

        public void SetOrderItems(List<OrderItem> items)
        {
            var @event = new OrderItemsUpdated(this, items);
            
            AddEvent(@event);
        }
        
        public void ConfirmOrder()
        {
            var @event = new OrderConfirmed(this);
            
            AddEvent(@event);
        }

        protected override void Apply(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case OrderCreated ctx:
                    Id = ctx.Id;
                    CustomerId = ctx.CustomerId;
                    Value = ctx.Value;
                    Status = OrderStatus.Draft.Name;
                    break;
                
                case OrderItemsUpdated ctx:
                    Items = ctx.Items;
                    break;
                
                case OrderConfirmed ctx:
                    Status = ctx.Status;
                    break;
            }
        }
    }
}