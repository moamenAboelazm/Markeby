using Library.Features.Bookings.Commands;
using Library.Features.Bookings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            try
            {
                var bookingId = await _mediator.Send(command);
                return Ok(new { Message = "Booking created successfully.", BookingId = bookingId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] UpdateBookingCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { Message = "ID in the URL does not match the ID in the body." });

            try
            {
                var result = await _mediator.Send(command);
                if (!result)
                    return NotFound(new { Message = "Booking not found." });

                return Ok(new { Message = "Booking updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPut("Cancel/{id}")]
        [Authorize]
        public async Task<IActionResult> CancelBooking(Guid id)
        {
            var result = await _mediator.Send(new CancelBookingCommand { Id = id });

            if (!result)
                return NotFound(new { Message = "Booking not found." });

            return Ok(new { Message = "Booking Cancelled successfully." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var result = await _mediator.Send(new DeleteBookingCommand { Id = id });

            if (!result)
                return NotFound(new { Message = "Booking not found." });

            return Ok(new { Message = "Booking deleted successfully." });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBookings([FromQuery] GetPagedBookingsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var result = await _mediator.Send(new GetBookingWithDetailsQuery { Id = id });

            if (result == null)
                return NotFound(new { Message = "Booking not found." });

            return Ok(result);
        }

        [HttpGet("system-dashboard")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSystemDashboard()
        {
            var result = await _mediator.Send(new GetSystemDashboardQuery());
            return Ok(result);
        }

        [HttpGet("trip-dashboard/{tripId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTripDashboard(Guid tripId)
        {
            var result = await _mediator.Send(new GetTripDashboardQuery { TripId = tripId });

            if (result == null)
                return NotFound(new { Message = "Trip not found." });

            return Ok(result);
        }
    }
}