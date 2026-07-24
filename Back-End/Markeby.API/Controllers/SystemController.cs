using Library.Features.Bookings.Queries;
using Library.Features.Dashboard.Queries;
using Library.Features.Trips.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DashboardController(IMediator _mediator) : ControllerBase
{
    [HttpGet("system-summary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSystemDashboard()
    {
        var result = await _mediator.Send(new GetSystemDashboardStatsQuery());
        return Ok(result);
    }

    [HttpGet("trips-for-user")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAvailableTripsForUser([FromQuery] GetPagedAvailableTripsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}