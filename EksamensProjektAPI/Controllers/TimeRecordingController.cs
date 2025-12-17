using Microsoft.AspNetCore.Mvc;
using EksamensProjektAPI.Services;

namespace EksamensProjektAPI.Controllers;

[ApiController]
[Route("api/timerecords")]
public class TimeRecordsController : ControllerBase // ControllerBase er en klasse fra ASP.NET Core
{
    private readonly TimeRecordingsService _service; // private field for TimeRecordingsService

    public TimeRecordsController(TimeRecordingsService service)
    {
        _service = service; // constructor injection så det kan bruges i metodernes kald
    }

    [HttpGet("user/{userId}")] // api for alle time records til en user
    public async Task<IActionResult> GetForUser(int userId)
    {
        var data = await _service.GetForUserAsync(userId);
        return Ok(data); // returnerer en liste med TimeRecordingModel's
    }

    [HttpPost("startrecord/{userid}/{taskid}")] // api for at starte en time recording vha. userid og taskid
    public async Task<IActionResult> StartTimeRecording(int userid, int taskid)
    {
        await _service.StartTimeRecording(userid, taskid);
        return Ok(); // returnerer 200 ok
    }

    [HttpPost("endrecord/{userid}/{taskid}")] // api for at slutte en time recording vha. userid og taskid
    public async Task<IActionResult> EndTimeRecording(int userid, int taskid)
    {
        await _service.EndTimeRecording(userid, taskid);
        return Ok(); // returnerer 200 ok
    }

}