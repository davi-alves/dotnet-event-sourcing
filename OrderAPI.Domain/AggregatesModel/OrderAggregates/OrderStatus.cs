using System;
using System.Collections.Generic;
using System.Linq;
using OrderAPI.Domain.SeedWork;

namespace OrderAPI.Domain.AggregatesModel.OrderAggregates
{
    public class OrderStatus : Enumeration
    {
        public static OrderStatus Draft = new OrderStatus(1, nameof(Draft).ToLowerInvariant());
        public static OrderStatus Pending = new OrderStatus(2, nameof(Pending).ToLowerInvariant());
        public static OrderStatus InSeparation = new OrderStatus(3, nameof(InSeparation).ToLowerInvariant());
        public static OrderStatus Billed = new OrderStatus(4, nameof(Billed).ToLowerInvariant());
        public static OrderStatus Sent = new OrderStatus(5, nameof(Sent).ToLowerInvariant());
        public static OrderStatus Delivered = new OrderStatus(6, nameof(Delivered).ToLowerInvariant());
        public static OrderStatus Complete = new OrderStatus(7, nameof(Complete).ToLowerInvariant());
        public static OrderStatus Refunded = new OrderStatus(8, nameof(Refunded).ToLowerInvariant());
        public static OrderStatus Canceled = new OrderStatus(9, nameof(Canceled).ToLowerInvariant());
        
        public OrderStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<OrderStatus> List() =>
            new[] {Draft, Pending, InSeparation, Billed, Sent, Delivered, Complete, Refunded, Canceled};

        public static OrderStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new Exception($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static OrderStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new Exception($"Possible values for OrderStatus: {String.Join(",", List().Select(s => s.Id))}");
            }

            return state;
        }
    }
}