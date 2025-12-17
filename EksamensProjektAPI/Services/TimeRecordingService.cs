using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class TimeRecordingsService
{
    private readonly TimeRecordingsRepo _repo = new(); // instansierer repo objektet

    public async Task<List<TimeRecordingModel>> GetForUserAsync(int userId) // henter alle time recordings for en bruger vha. bruger id
        => await _repo.GetAllForUserAsync(userId); // kald repo funktionen

    public async Task StartTimeRecording(int userid, int taskid) // starter en time recording vha. bruger id og task id
        => await _repo.StartTimeRecording(userid, taskid); // kald repo funktionen
    
    public async Task EndTimeRecording(int userid, int taskid) // slutter en time recording vha. bruger id og task id
    => await _repo.EndTimeRecording(userid, taskid); // kald repo funktionen

}