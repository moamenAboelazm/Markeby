using Library.Features.Boats.Queries;
using Library.Features.Trips.Commands;
using Library.Features.Trips.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Markeby.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTrip([FromForm] CreateTripCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = "Trip created successfully", Id = result });
        }

        [HttpGet("available-resources")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAvailableResources([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            if (startTime >= endTime)
                return BadRequest(new { Message = "Start time must be before End Time" });
            
            if (startTime < DateTime.UtcNow.AddHours(3))
                return BadRequest(new { Message = "A trip cannot be created for a past time." });

            var result = await _mediator.Send(new GetAvailableResourcesQuery
            {
                StartTime = startTime,
                EndTime = endTime
            });

            return Ok(result);
        }

        [HttpGet("dashboard-stats")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var result = await _mediator.Send(new GetTripDashboardStatsQuery());
            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBoats([FromQuery] GetPagedTripsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTripById(Guid id)
        {
            var result = await _mediator.Send(new GetTripWithDetailsByIdQuery { Id = id });

            if (result == null)
                return NotFound(new { Message = "Trip not found." });

            return Ok(result);
        }

        [HttpGet("check-seats")]
        public async Task<IActionResult> CheckAvailableSeats([FromQuery] CheckAvailableSeatsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new { HasAvailableSeats = result });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTrip(Guid id, [FromForm] UpdateTripCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Trip not found." });

            return Ok(new { Message = "Trip updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var result = await _mediator.Send(new DeleteTripCommand { Id = id });

            if (!result)
                return NotFound(new { Message = "Trip not found." });

            return Ok(new { Message = "Trip deleted successfully" });
        }
    }
}