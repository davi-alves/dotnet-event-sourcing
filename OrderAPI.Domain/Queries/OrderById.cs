using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderApi.API.DTOs;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Infrastructure.Repositores;

namespace OrderApi.Domain.Queries
{
    public class OrderById
    {
        public class Query : IRequest<OrderDetails>
        {
            public Query(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; }
        }

        public class Handler : IRequestHandler<Query, OrderDetails>
        {
            private readonly IReadStoreRepository<Order, Guid> _repository;

            public Handler(IReadStoreRepository<Order, Guid> repository)
            {
                _repository = repository;
            }

            public async Task<OrderDetails> Handle(Query request, CancellationToken cancellationToken)
            {
                var order = await _repository.GetAsync(request.Id);
                if (order == null)
                    return null;
                
                var orderDetails = new OrderDetails(
                    order.Id,
                    order.CustomerId,
                    order.Value,
                    order.Status,
                    order.Items
                );

                return orderDetails;
            }
        }
    }
}