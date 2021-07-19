using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.API.DTOs;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Domain.Commands;
using OrderApi.Domain.Queries;

namespace OrderApi.API
{
    [ApiController]
    [Route("api/v1/order")]
    public class OrderController : BaseApiController
    {
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
            /*
             * 1. Receive the message from SNS
             * 2. Parse from plain text
             * 3. Retrieve the message payload
             * 4. Parse into CreateOrderDto
             * 5. Call the Command
             * 6. Return a confirmation
             */

            var command =
                new CreateOrder.Command(dto.TransactionId, dto.CustomerId, dto.Value, dto.Items, dto.Confirmed);
            await Mediator.Send(command);
            var result = new {Id = command.Id};

            return CreatedAtAction("GetOrder", result, result);
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncOrderFromCosmos()
        {
            /*
             * 1. Receive the message from SNS
             * 2. Parse from plain text
             * 3. Retrieve the message payload
             * 4. Parse into a Dto (basic - v0)
             * 5. Call the Command
             * 6. Return a confirmation
             */

            var command =
                new SyncOrderFromCosmos.Command();
            await Mediator.Send(command);
            var result = new {Id = command.Id};

            return CreatedAtAction("GetOrder", result, result);
        }
    }
}