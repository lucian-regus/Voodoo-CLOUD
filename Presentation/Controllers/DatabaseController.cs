using Infrastructure.DTOs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("/api/database")]
public class DatabaseController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public DatabaseController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet("updates")]
    public async Task<ActionResult<DatabaseUpdateResponse>> GetDatabaseUpdatesAsync([FromQuery] DateTime delta)
    {
        var response = await _databaseService.GetDatabaseUpdatesAsync(delta);

        return Ok(response);
    }
}