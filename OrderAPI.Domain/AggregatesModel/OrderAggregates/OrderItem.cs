using System;
using Amazon.DynamoDBv2.DataModel;

namespace OrderAPI.Domain.AggregatesModel.OrderAggregates
{
    public class OrderItem
    {
        [DynamoDBProperty("sku")]
        public string Sku { get; set; }
        
        [DynamoDBProperty("quantity")]
        public int Quantity { get; set; }
        
        [DynamoDBProperty("finalPrice")]
        public Decimal FinalPrice { get; set; }
    }
}