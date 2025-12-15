using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class TimeRecordingsService
{
    private readonly TimeRecordingsRepo _repo = new();

    public async Task<List<TimeRecordingModel>> GetForUserAsync(int userId)
        => await _repo.GetAllForUserAsync(userId);

    public async Task StartTimeRecording(int userid, int taskid)
        => await _repo.StartTimeRecording(userid, taskid);
    
    public async Task EndTimeRecording(int userid, int taskid)
    => await _repo.EndTimeRecording(userid, taskid);

}