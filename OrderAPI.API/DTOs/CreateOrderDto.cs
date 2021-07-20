using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;

namespace OrderApi.API.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public Guid TransactionId { get; set; }
        
        [Required]
        public Guid CustomerId { get; set; }
        
        [Required]
        public decimal Value { get; set; }
        
        public List<OrderItem> Items { get; set; }

        public bool Confirmed { get; set; } = false;
    }
}