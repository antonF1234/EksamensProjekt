using EksamensProjekt.Models;
using Npgsql;
namespace EksamensProjektAPI.Repositories;

public class UsersTasksRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;
    
    public async Task InsertAsync(int taskId, int userId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand(
            "INSERT INTO users_tasks (user_id, task_id) VALUES (@user_id, @task_id)",
            conn);

        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("task_id", taskId);
        
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<List<UsersTasksModel>> GetAllUserTasksAsync(int userId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        var sql = @"
        SELECT 
            ut.user_task_id, 
            ut.user_id, 
            ut.task_id, 
            t.name AS task_name, 
            u.username AS user_name,
            t.start_date,
            t.deadline,
            t.completion_date,
            t.status
        FROM users_tasks ut
        JOIN tasks t ON ut.task_id = t.task_id
        JOIN users u ON ut.user_id = u.user_id
        WHERE ut.user_id = @uid";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("uid", userId);

        await using var reader = await cmd.ExecuteReaderAsync();
        var usersTasks = new List<UsersTasksModel>();

        while (await reader.ReadAsync())
        {
            usersTasks.Add(new UsersTasksModel
            {
                UserTaskId = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                TaskId = reader.GetInt32(2),
                TaskName = reader.GetString(3),
                UserName = reader.GetString(4),
                StartDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                Deadline = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                CompletionDate = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                Status = reader.IsDBNull(8) ? "" : reader.GetString(8)
            });
        }

        return usersTasks;
    }
    
}