using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderApi.API.DTOs;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Domain.Queries;
using OrderApi.Infrastructure.Repositores;

namespace OrderApi.OrderAPI.Domain.Handlers
{
    public class OrderByIdHandler: IRequestHandler<OrderByIdQuery, OrderDetails>
    {
        private readonly IReadStoreRepository<Order, Guid> _repository;

        public OrderByIdHandler(IReadStoreRepository<Order, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<OrderDetails> Handle(OrderByIdQuery request, CancellationToken cancellationToken)
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