using System.ComponentModel.DataAnnotations;
using Infrastructure.DTOs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("/api/threads")]
public class ThreatsController : ControllerBase
{
    private readonly IThreatsService _threatsService;

    public ThreatsController(IThreatsService threatsService)
    {
        _threatsService = threatsService;
    }

    [HttpGet("ips/{ip}")]
    public async Task<ActionResult<ExistsResponse>> CheckIpExistsAsync([FromRoute] string ip)
    {
        var response = await _threatsService.CheckIpExistsAsync(ip);

        return Ok(response);
    }
    
    [HttpGet("signatures/{hash}")]
    public async Task<ActionResult<ExistsResponse>> CheckSignatureExistsAsync([FromRoute] string hash)
    {
        var response = await _threatsService.CheckSignatureExistsAsync(hash);
        
        return Ok(response);
    }
}