using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Library.Features.Trips.Commands;
using Library.Features.Trips.Queries;

namespace Markeby.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TripsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromForm] CreateTripCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = "Trip created successfully", Id = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTrips([FromQuery] GetAllTripsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
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
        public async Task<IActionResult> UpdateTrip(Guid id, [FromForm] UpdateTripCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Trip not found." });

            return Ok(new { Message = "Trip updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(Guid id)
        {
            var result = await _mediator.Send(new DeleteTripCommand { Id = id });

            if (!result)
                return NotFound(new { Message = "Trip not found." });

            return Ok(new { Message = "Trip deleted successfully" });
        }
    }
}