using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Infrastructure.EventStore;

namespace OrderApi.Domain.Commands
{
    public class SyncOrderFromCosmosCommand : IRequest
    {
        public SyncOrderFromCosmosCommand()
        {
            // TODO: implement this with correct properties
        }

        public Guid Id { get; set; }
        public string BranchName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Decimal TotalValue { get; set; }
        public Decimal DiscountValue { get; set; }
    }
}