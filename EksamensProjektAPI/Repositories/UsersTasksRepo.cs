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
            t.project_id,
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
                ProjectId = reader.GetInt32(4),
                UserName = reader.GetString(5),
                StartDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                Deadline = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                CompletionDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                Status = reader.IsDBNull(9) ? "" : reader.GetString(9)
            });
        }

        return usersTasks;
    }
    
    public async Task<List<UserModel>> GetUsersByTaskIdAsync(int taskId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        var users = new List<UserModel>();

        await using var cmd = new NpgsqlCommand(@"
        SELECT u.user_id, u.username
        FROM users u
        INNER JOIN users_tasks ut ON u.user_id = ut.user_id
        WHERE ut.task_id = @TaskId
    ", conn);

        cmd.Parameters.AddWithValue("TaskId", taskId);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            users.Add(new UserModel
            {
                UserId = reader.GetInt32(0),
                Username = reader.GetString(1)
            });
        }

        return users;
    }
    
    public async Task DelUserFromTask(int taskId, int userId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand(
            "DELETE FROM users_tasks WHERE user_id = @user_id AND task_id = @task_id",
            conn);

        cmd.Parameters.AddWithValue("user_id", userId);
        cmd.Parameters.AddWithValue("task_id", taskId);
        
        await cmd.ExecuteNonQueryAsync();
    }
}