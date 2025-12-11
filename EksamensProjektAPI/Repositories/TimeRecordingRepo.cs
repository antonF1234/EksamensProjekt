using EksamensProjekt.Models;
using Npgsql;

namespace EksamensProjektAPI.Repositories;

public class TimeRecordingsRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    public async Task<List<TimeRecordingModel>> GetAllForUserAsync(int userId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        var sql = @"
            SELECT 
                tr.time_record_id,
                tr.user_id,
                tr.task_id,
                u.username,
                t.name,
                tr.start_time,
                tr.end_time
            FROM time_recording tr
            JOIN users u ON tr.user_id = u.user_id
            JOIN tasks t ON tr.task_id = t.task_id
            WHERE tr.user_id = @uid
            ORDER BY tr.start_time DESC;
        ";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("uid", userId);

        await using var reader = await cmd.ExecuteReaderAsync();

        var list = new List<TimeRecordingModel>();

        while (await reader.ReadAsync())
        {
            list.Add(new TimeRecordingModel
            {
                TimeRecordId = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                TaskId = reader.GetInt32(2),
                UserName = reader.GetString(3),
                TaskName = reader.GetString(4),
                StartTime = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                EndTime = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
            });
        }

        return list;
    }
}