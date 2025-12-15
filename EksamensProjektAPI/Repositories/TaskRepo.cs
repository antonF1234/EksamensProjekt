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
            "SELECT task_id, name, description, start_date, deadline, completion_date, status, project_id FROM tasks ORDER BY task_id",
            conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        var tasks = new List<TaskModel>();

        while (await reader.ReadAsync())
        {
            tasks.Add(new TaskModel
            {
                TaskId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                StartDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                Deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                CompletionDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                Status = reader.IsDBNull(6) ? null : reader.GetString(6),
                ProjectId = reader.GetInt32(7)
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
            "SELECT task_id, name, description, start_date, deadline, completion_date, status, project_id FROM tasks WHERE task_id = @id",
            conn);

        cmd.Parameters.AddWithValue("id", tid);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new TaskModel
        {
            TaskId = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
            StartDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
            Deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
            CompletionDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
            Status = reader.IsDBNull(6) ? null : reader.GetString(6),
            ProjectId = reader.GetInt32(7)
        };
    }

    public async Task<List<TaskModel>> GetByProjectIdAsync(int pid)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT task_id, name, description, start_date, deadline, completion_date, status, project_id FROM tasks WHERE project_id = @pid ORDER BY task_id",
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
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                StartDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                Deadline = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                CompletionDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                Status = reader.IsDBNull(6) ? null : reader.GetString(6),
                ProjectId = reader.GetInt32(7)
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
            @"INSERT INTO tasks 
              (name, description, start_date, deadline, completion_date, status, project_id) 
              VALUES (@name, @description, @start_date, @deadline, @completion_date, @status, @project_id)",
            conn);

        cmd.Parameters.AddWithValue("name", task.Name);
        cmd.Parameters.AddWithValue("description", (object?)task.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("start_date", (object?)task.StartDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("deadline", (object?)task.Deadline ?? DBNull.Value);
        cmd.Parameters.AddWithValue("completion_date", (object?)task.CompletionDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("status", (object?)task.Status ?? "Ny");
        cmd.Parameters.AddWithValue("project_id", task.ProjectId);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateTaskStatusAsync(int taskId, string status)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

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

    public async Task UpdateAsync(TaskModel task)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        var sql = @"
        UPDATE tasks 
        SET name=@name,
            description=@description,
            start_date=@startdate,
            deadline=@deadline,
            completion_date=@completiondate,
            status=@status,
            project_id=@projectid
        WHERE task_id=@tid";

        await using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("name", task.Name);
        cmd.Parameters.AddWithValue("description", (object?)task.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("startdate", (object?)task.StartDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("deadline", (object?)task.Deadline ?? DBNull.Value);
        cmd.Parameters.AddWithValue("completiondate", (object?)task.CompletionDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("status", (object?)task.Status ?? "Ny");
        cmd.Parameters.AddWithValue("projectid", task.ProjectId);
        cmd.Parameters.AddWithValue("tid", task.TaskId);

        await cmd.ExecuteNonQueryAsync();
    }
}
