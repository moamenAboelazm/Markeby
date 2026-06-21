using Library.Dashboards.Captains;
using Library.Features.Bookings.Commands;
using Library.Features.Captains.Commands;
using Library.Features.Captains.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptainsController(IMediator _mediator) : ControllerBase
    {

        [Authorize(Roles = "Admin")]
        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _mediator.Send(new GetCaptainDashboardStatsQuery());
            return Ok(stats);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedCaptains([FromQuery] GetPagedCaptainsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCaptain(Guid id)
        {
            var query = new GetCaptainWithBoatsAndTripsQuery { Id = id };
            var captain = await _mediator.Send(query);

            if (captain == null)
                return NotFound(new { Message = "Captain not found." });

            return Ok(captain);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCaptain([FromForm] CreateCaptainCommand command)
        {
            var captainId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCaptain), new { id = captainId }, new { Id = captainId, Message = "Captain profile created successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCaptain(Guid id, [FromForm] UpdateCaptainCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { Message = "ID mismatch." });

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Captain not found." });

            return Ok(new { Message = "Captain profile updated successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCaptain(Guid id)
        {
            var result = await _mediator.Send(new DeleteCaptainCommand { Id = id });

            if (!result)
                return NotFound(new { Message = "Captain not found." });

            return Ok(new { Message = "Captain deleted successfully" });
        }
    }
}