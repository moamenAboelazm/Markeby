using Library.Dashboards.Captains;
using Library.Features.Captains.Commands;
using Library.Features.Captains.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptainsController(IMediator _mediator) : ControllerBase
    {

        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _mediator.Send(new GetCaptainDashboardStatsQuery());
            return Ok(stats);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedCaptains([FromQuery] GetPagedCaptainsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCaptain(Guid id)
        {
            var query = new GetCaptainWithBoatsAndTripsQuery { Id = id };
            var captain = await _mediator.Send(query);

            if (captain == null)
                return NotFound(new { Message = "Captain not found." });

            return Ok(captain);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCaptain([FromForm] CreateCaptainCommand command)
        {
            var captainId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetCaptain), new { id = captainId }, new { Id = captainId, Message = "Captain profile created successfully." });
        }

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
    }
}