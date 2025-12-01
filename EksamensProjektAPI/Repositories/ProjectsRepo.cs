using Npgsql;
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class ProjectRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    public async Task<List<ProjectModel>> GetAllAsync()
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT project_id, name, description, deadline, status FROM projects ORDER BY project_id", conn);

        await using var reader = await cmd.ExecuteReaderAsync();
        var projects = new List<ProjectModel>();

        while (await reader.ReadAsync())
        {
            projects.Add(new ProjectModel
            {
                ProjectId = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                Deadline =  reader.GetDateTime(3),
                Status = reader.GetString(4)
            });
        }

        return projects;
    }

    public async Task<ProjectModel?> GetByIdAsync(int id)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT project_id, name, description, deadline, status FROM projects WHERE project_id = @id LIMIT 1", conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        if (!reader.HasRows) return null;

        return new ProjectModel
        {
            ProjectId = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            Deadline =  reader.GetDateTime(3),
            Status = reader.GetString(4)
        };
    }

    public async Task InsertAsync(ProjectModel project)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO projects (name, description, deadline, status) VALUES (@name, @desc, @deadline, @status)", conn);

        cmd.Parameters.AddWithValue("name", project.Name);
        cmd.Parameters.AddWithValue("desc", project.Description);
        cmd.Parameters.AddWithValue("deadline", project.Deadline);
        cmd.Parameters.AddWithValue("status", project.Status);

        await cmd.ExecuteNonQueryAsync();
    }
}
