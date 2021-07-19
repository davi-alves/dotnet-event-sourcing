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
    public class OrderByIdQuery : IRequest<OrderDetails>
    {
        public OrderByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}