using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Library.Features.Boats.Commands;
using Library.Features.Boats.Queries;

namespace Markeby.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BoatsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateBoat([FromForm] CreateBoatCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = "Boat created successfully", Id = result });
        }


        [HttpGet("active")]
        public async Task<IActionResult> GetAllActiveBoats([FromQuery] GetAllActiveBoatsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoatById(Guid id)
        {
            var result = await _mediator.Send(new GetBoatByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { Message = "Boat not found." });

            return Ok(result);
        }

        [HttpGet("capacity/{minimumCapacity}")]
        public async Task<IActionResult> GetBoatsByCapacity(int minimumCapacity)
        {
            var result = await _mediator.Send(new GetBoatsByCapacityQuery { MinimumCapacity = minimumCapacity });
            return Ok(result);
        }

        [HttpGet("{id}/availability")]
        public async Task<IActionResult> CheckBoatAvailability(Guid id, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            var result = await _mediator.Send(new CheckBoatAvailabilityQuery
            {
                BoatId = id,
                StartTime = startTime,
                EndTime = endTime
            });

            return Ok(new { IsAvailable = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoat(Guid id, [FromForm] UpdateBoatCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Boat not found." });

            return Ok(new { Message = "Boat updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoat(Guid id)
        {
            var result = await _mediator.Send(new DeleteBoatCommand { Id = id });

            if (!result)
                return NotFound(new { Message = "Boat not found." });

            return Ok(new { Message = "Boat deleted successfully" });
        }
    }
}