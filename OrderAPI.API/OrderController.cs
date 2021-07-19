using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderApi.API.DTOs;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Domain.Commands;
using OrderApi.Domain.Queries;
using OrderApi.Infrastructure.Persistence;

namespace OrderApi.API
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : BaseApiController
    {
        private readonly ILogger<OrderController> _logger;
        private readonly EntityEventContext _context;

        public OrderController(ILogger<OrderController> logger, EntityEventContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var query = new OrderById.Query(id);
            var result = await Mediator.Send(query);
            if (null == result)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command =
                new CreateOrder.Command(dto.TransactionId, dto.CustomerId, dto.Value, dto.Items, dto.Confirmed);
            await Mediator.Send(command);
            var result = new {Id = command.Id};

            return CreatedAtAction("GetOrder", result, result);
        }
    }
}