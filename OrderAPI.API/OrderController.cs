using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.API.DTOs;
using OrderAPI.Infrastructure.Core.SnsClasses;
using OrderAPI.Domain.AggregatesModel.OrderAggregates;
using OrderApi.Domain.Commands;
using OrderApi.Domain.Queries;
using Newtonsoft.Json;

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
            // all of this logic can be moved to the command and let the controller better
            var snsMessage = JsonConvert.DeserializeObject<AwsSnsMessage>(await Request.GetRawBodyStringAsync());

            if (snsMessage?.SubscribeURL != null)
            {
                return Ok(snsMessage.SubscribeURL);
            }
            
            if (snsMessage?.Message == null)
            {
                return BadRequest("No order was sent in the request.");
            }
            
            var dto = JsonConvert.DeserializeObject<CreateOrderDto>(snsMessage.Message);
            
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
            var snsMessage = JsonConvert.DeserializeObject<AwsSnsMessage>(await Request.GetRawBodyStringAsync());

            if (snsMessage?.SubscribeURL != null)
            {
                return Ok(snsMessage.SubscribeURL);
            }
            
            if (snsMessage?.Message == null)
            {
                return BadRequest("No order was sent in the request.");
            }
            
            var dto = JsonConvert.DeserializeObject<SyncOrderDto>(snsMessage.Message);

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