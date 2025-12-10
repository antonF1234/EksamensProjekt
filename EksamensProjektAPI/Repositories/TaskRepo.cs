using Npgsql;
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class TaskRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    // GET alle tasks
    public async Task<List<TaskModel>> GetAllAsync()
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT task_id, name, start_date, deadline, completion_date, status, project_id FROM tasks ORDER BY task_id",
            conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        var tasks = new List<TaskModel>();

        while (await reader.ReadAsync())
        {
            tasks.Add(new TaskModel
            {
                TaskId = reader.GetInt32(0),
                Name = reader.GetString(1),
                StartDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                Deadline = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                CompletionDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                Status = reader.IsDBNull(5) ? null : reader.GetString(5),
                ProjectId = reader.GetInt32(6)
            });
        }

        return tasks;
    }

    // GET en task
    public async Task<TaskModel?> GetByIdAsync(int tid)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT task_id, name, start_date, deadline, completion_date, status, project_id FROM tasks WHERE task_id = @id",
            conn);

        cmd.Parameters.AddWithValue("id", tid);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new TaskModel
        {
            TaskId = reader.GetInt32(0),
            Name = reader.GetString(1),
            StartDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
            Deadline = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
            CompletionDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
            Status = reader.IsDBNull(5) ? null : reader.GetString(5),
            ProjectId = reader.GetInt32(6)
        };
    }
    
    public async Task<List<TaskModel>> GetByProjectIdAsync(int pid)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT task_id, name, start_date, deadline, completion_date, status, project_id FROM tasks WHERE project_id = @pid ORDER BY project_id",
            conn);
        
        cmd.Parameters.AddWithValue("pid", pid);

        await using var reader = await cmd.ExecuteReaderAsync();
        var tasks = new List<TaskModel>();

        while (await reader.ReadAsync())
        {
            tasks.Add(new TaskModel
            {
                TaskId = reader.GetInt32(0),
                Name = reader.GetString(1),
                StartDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2),
                Deadline = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                CompletionDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                Status = reader.IsDBNull(5) ? null : reader.GetString(5),
                ProjectId = reader.GetInt32(6)
            });
        }

        return tasks;
    }

    // POST – indsæt task
    public async Task InsertAsync(TaskModel task)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO tasks (name, start_date, deadline, completion_date, status, project_id) VALUES (@name, @start_date, @deadline, @completion_date, @status, @project_id)",
            conn);

        cmd.Parameters.AddWithValue("name", task.Name);
        cmd.Parameters.AddWithValue("start_date", (object?)task.StartDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("deadline", (object?)task.Deadline ?? DBNull.Value);
        cmd.Parameters.AddWithValue("completion_date", (object?)task.CompletionDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("status", (object?)task.Status ?? "Ny");
        cmd.Parameters.AddWithValue("project_id", task.ProjectId);

        await cmd.ExecuteNonQueryAsync();
    }
    
    public async Task UpdateTaskAsync(int taskId, string status)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // 1. opdater taskens status udfra id
        await using var cmd = new NpgsqlCommand(
            "UPDATE tasks SET status=@status WHERE task_id=@taskId",
            conn);
        
        cmd.Parameters.AddWithValue("taskId", taskId);
        cmd.Parameters.AddWithValue("status", status);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand(
            "DELETE FROM tasks WHERE task_id=@taskId",
            conn);
        
        cmd.Parameters.AddWithValue("taskId", taskId);
        
        await cmd.ExecuteNonQueryAsync();
    }
}
