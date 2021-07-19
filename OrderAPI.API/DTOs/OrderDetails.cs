using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderAPI.Infrastructure.Core;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.API.DTOs
{
    
    public class OrderDetails : IEntity<Guid>
    {
        public OrderDetails(Guid id, Guid customerId, Decimal value, string status, List<OrderItem> items)
        {
            Id = id;
            CustomerId = customerId;
            Value = value;
            Status = status;
            Items = items;
        }
        
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Value { get; set; }
        public string Status { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}