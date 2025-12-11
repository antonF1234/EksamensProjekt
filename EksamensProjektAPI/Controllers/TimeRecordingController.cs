using Microsoft.AspNetCore.Mvc;
using EksamensProjektAPI.Services;

namespace EksamensProjektAPI.Controllers;

[ApiController]
[Route("api/timerecords")]
public class TimeRecordsController : ControllerBase
{
    private readonly TimeRecordingsService _service;

    public TimeRecordsController(TimeRecordingsService service)
    {
        _service = service;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetForUser(int userId)
    {
        var data = await _service.GetForUserAsync(userId);
        return Ok(data);
    }
}