using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.API.DTOs;
using OrderAPI.Infrastructure.Core.SnsClasses;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Domain.Commands;
using OrderApi.Domain.Queries;
using System.Text.Json;

namespace OrderApi.API
{
    [ApiController]
    [Route("api/v1/order")]
    public class OrderController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var query = new OrderByIdQuery(id);
            var result = await Mediator.Send(query);
            if (null == result)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("CreateOrder")]
        [Consumes("text/plain")]
        public async Task<IActionResult> CreateOrder()
        {
            var snsMessage = JsonSerializer.Deserialize<AwsSnsMessage>(await Request.GetRawBodyStringAsync());

            if (snsMessage?.Message == null)
            {
                return BadRequest("No order was sent in the request.");
            }
            
            var dto = JsonSerializer.Deserialize<CreateOrderDto>(snsMessage.Message);
            
            if (dto == null)
            {
                // add FluentValidation to handle this errors
                return BadRequest("No order was sent in the request.");
            }
            
            var command =
                new CreateOrderCommand(dto.TransactionId, dto.CustomerId, dto.Value, dto.Items, dto.Confirmed);
            await Mediator.Send(command);
            var result = new {Id = command.Id};

            return CreatedAtAction("GetOrder", result, result);
        }

        [HttpPost("SyncOrderFromCosmos")]
        [Consumes("text/plain")]
        public async Task<IActionResult> SyncOrderFromCosmos()
        {
            var snsMessage = JsonSerializer.Deserialize<AwsSnsMessage>(await Request.GetRawBodyStringAsync());

            if (snsMessage?.Message == null)
            {
                return BadRequest("No order was sent in the request.");
            }
            
            var dto = JsonSerializer.Deserialize<SyncOrderDto>(snsMessage.Message);

            if (dto == null)
            {
                // add FluentValidation to handle this errors
                return BadRequest("No order was sent in the request.");
            }

            var command =
                new SyncOrderFromCosmosCommand();
            await Mediator.Send(command);

            return NoContent();
        }
        
        [HttpPost("CreateOrderTest")]
        public async Task<IActionResult> CreateOrderTest(CreateOrderDto dto)
        {
            var command =
                new CreateOrderCommand(dto.TransactionId, dto.CustomerId, dto.Value, dto.Items, dto.Confirmed);
            await Mediator.Send(command);
            var result = new {Id = command.Id};

            return CreatedAtAction("GetOrder", result, result);
        }
    }
}