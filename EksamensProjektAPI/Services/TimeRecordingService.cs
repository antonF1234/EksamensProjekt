using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class TimeRecordingsService
{
    private readonly TimeRecordingsRepo _repo = new();

    public async Task<List<TimeRecordingModel>> GetForUserAsync(int userId)
        => await _repo.GetAllForUserAsync(userId);
}