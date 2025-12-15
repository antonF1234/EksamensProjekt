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

    public async Task StartTimeRecording(int userId, int taskId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO time_recording (user_id, task_id, start_time) " +
            "SELECT @userid, @taskid, NOW() " +
            "WHERE NOT EXISTS (" +
            "   SELECT 1 FROM time_recording " +
            "   WHERE user_id = @userid AND end_time IS NULL AND sum_of_time_second IS NULL" +
            ")",
            conn);

        cmd.Parameters.AddWithValue("userid", userId);
        cmd.Parameters.AddWithValue("taskid", taskId);

        await cmd.ExecuteNonQueryAsync();
    }


    public async Task EndTimeRecording(int userId, int taskId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "UPDATE time_recording SET end_time = NOW(), sum_of_time_second = EXTRACT(EPOCH FROM (NOW() - start_time))::INT WHERE user_id = @userid AND task_id = @taskid AND end_time IS NULL",
            conn);

        cmd.Parameters.AddWithValue("userid", userId);
        cmd.Parameters.AddWithValue("taskid", taskId);

        await cmd.ExecuteNonQueryAsync();
    }

}