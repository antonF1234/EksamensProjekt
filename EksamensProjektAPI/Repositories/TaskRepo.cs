using Npgsql;
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class TaskRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;
    
    public async Task<List<TaskModel>> GetAllAsync()
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT task_id, name, start_date, deadline, completion_date, status, project_id FROM task ORDER BY task_id", conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        var projects = new List<TaskModel>();

        while (await reader.ReadAsync())
        {
            projects.Add(new TaskModel
            {
                TaskId = reader.GetInt32(0),
                Name = reader.GetString(1),
                StartDate = reader.GetDateTime(2),
                Deadline =  reader.GetDateTime(3),
                CompletionDate = reader.GetDateTime(4),
                Status = reader.GetString(4),
                ProjectId = reader.GetInt32(5)
            });
        }

        return projects;
    }
    public async Task<TaskModel?> GetByIdAsync(int pid)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
                "SELECT task_id, name, start_date, deadline, completion_date, status, project_id FROM task ORDER BY project_id= @pid", conn);
        cmd.Parameters.AddWithValue("id", pid);

        await using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        if (!reader.HasRows) return null;

        return new TaskModel
        {
            TaskId = reader.GetInt32(0),
            Name = reader.GetString(1),
            StartDate = reader.GetDateTime(2),
            Deadline =  reader.GetDateTime(3),
            CompletionDate = reader.GetDateTime(4),
            Status = reader.GetString(4),
            ProjectId = reader.GetInt32(5)
        };
    }
    
    public async Task InsertAsync(TaskModel task)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // 1. Indsæt task
        await using var cmd = new NpgsqlCommand(
            "INSERT INTO tasks (name, start_date, deadline, completion_date, status, project_id) VALUES (@name, @start_date, @deadline, @completion_date, @status, @project_id)",
            conn);

        cmd.Parameters.AddWithValue("name", task.Name);
        cmd.Parameters.AddWithValue("desc", (object?)task.StartDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("deadline", (object?)task.Deadline ?? DBNull.Value);
        cmd.Parameters.AddWithValue("completion_date", (object?)task.CompletionDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("status", (object?)task.Status ?? "Ny");
        cmd.Parameters.AddWithValue("project_id", task.ProjectId);
    }
}