using Library.Features.Bookings.Commands;
using Library.Features.Bookings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Markeby.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingsController(IMediator _mediator) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = "Booking created successfully", Id = result });
        }

        [HttpPost("admin-book")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminCreateBooking([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new { Message = "Booking created successfully by Admin", Id = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings([FromQuery] GetAllBookingsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var result = await _mediator.Send(new GetBookingWithDetailsQuery { Id = id });

            if (result == null)
                return NotFound(new { Message = "Booking not found." });

            return Ok(result);
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckUserBookedTrip([FromQuery] CheckUserBookedTripQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(new { HasBooked = result });
        }

        [HttpGet("trip/{tripId}/revenue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTotalRevenueByTripId(Guid tripId)
        {
            var result = await _mediator.Send(new GetTotalRevenueByTripIdQuery { TripId = tripId });
            return Ok(new { TotalRevenue = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] UpdateBookingCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Booking not found or cannot be updated." });

            return Ok(new { Message = "Booking updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var result = await _mediator.Send(new DeleteBookingCommand { Id = id });

            if (!result)
                return NotFound(new { Message = "Booking not found." });

            return Ok(new { Message = "Booking deleted successfully" });
        }
    }
}