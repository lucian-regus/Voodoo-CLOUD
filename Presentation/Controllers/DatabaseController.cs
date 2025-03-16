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

    [HttpGet("updates/{delta}")]
    public async Task<ActionResult<DatabaseUpdateResponse>> GetDatabaseUpdatesAsync([FromRoute] DateTime delta)
    {
        var response = await _databaseService.GetDatabaseUpdatesAsync(delta);

        return Ok(response);
    }
}