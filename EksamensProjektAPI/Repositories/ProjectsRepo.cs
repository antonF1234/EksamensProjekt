using Npgsql;
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class ProjectRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    public async Task<List<ProjectModel>> GetAllAsync() // Henter alle projekter fra databasen. Retunere en liste af projekter
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand( // SQL-forespørgsel der henter alle projekter sorteret efter ID
            "SELECT project_id, name, description, deadline, status FROM projects ORDER BY project_id", conn);

        await using var reader = await cmd.ExecuteReaderAsync();   // Udfører forespørgslen og læser resultatet
        var projects = new List<ProjectModel>();

        while (await reader.ReadAsync()) // Mapper hver række fra databasen til et ProjectModel-objekt
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

    public async Task<ProjectModel?> GetByIdAsync(int id) // Henter ét projekt ud fra projektets ID. Retunere projekt hvis det findes, ellers null
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand( // SQL med parameter for at undgå SQL injection. @id LIMIT 1 begrænser antallet af rækker, som forespørgslen returnerer, til kun 1 række.
            "SELECT project_id, name, description, deadline, status FROM projects WHERE project_id = @id LIMIT 1", conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        if (!reader.HasRows) return null; // Returnerer null hvis projektet ikke findes

        return new ProjectModel
        {
            ProjectId = reader.GetInt32(0),
            Name = reader.GetString(1),
            Description = reader.GetString(2),
            Deadline =  reader.GetDateTime(3),
            Status = reader.GetString(4)
        };
    }

    public async Task InsertAsync(ProjectModel project, int userId) // Indsætter et nyt projekt og forbinder det til en bruger.
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // 1. Indsæt projekt i projects-tabellen
        await using var cmd = new NpgsqlCommand(
            "INSERT INTO projects (name, description, deadline, status) VALUES (@name, @desc, @deadline, @status) RETURNING project_id",
            conn);
        // Parametre anvendes for sikkerhed og korrekt håndtering af null-værdier
        cmd.Parameters.AddWithValue("name", project.Name);
        cmd.Parameters.AddWithValue("desc", (object?)project.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("deadline", (object?)project.Deadline ?? DBNull.Value);
        cmd.Parameters.AddWithValue("status", (object?)project.Status ?? "Ny");

        int projectId = (int)await cmd.ExecuteScalarAsync(); // Henter ID på det nyoprettede projekt

        // 2. Opret relation mellem bruger og projekt
        await using var cmd2 = new NpgsqlCommand(
            "INSERT INTO users_projects (user_id, project_id) VALUES (@userId, @projectId)",
            conn);

        cmd2.Parameters.AddWithValue("userId", userId);
        cmd2.Parameters.AddWithValue("projectId", projectId);

        await cmd2.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(ProjectModel project) // Sletter et projekt fra databasen.
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // SQL-kommando der sletter projekt baseret på ID
        await using var cmd = new NpgsqlCommand(
            "DELETE FROM projects WHERE project_id = @pid",
            conn);

        cmd.Parameters.AddWithValue("pid", project.ProjectId);
        
        await cmd.ExecuteNonQueryAsync();
    }
    
    public async Task UpdateAsync(ProjectModel project) // Opdaterer et eksisterende projekt i databasen.
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // SQL-kommando der opdaterer projektets felter
        await using var cmd = new NpgsqlCommand(
            "UPDATE projects SET name=@name, description=@desc, deadline=@deadline, status=@status WHERE project_id=@pid",
            conn);
        
        cmd.Parameters.AddWithValue("name", project.Name);
        cmd.Parameters.AddWithValue("desc", (object?)project.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("deadline", (object?)project.Deadline ?? DBNull.Value);
        cmd.Parameters.AddWithValue("status", (object?)project.Status);
        cmd.Parameters.AddWithValue("pid", project.ProjectId);

        await cmd.ExecuteNonQueryAsync();
    }
}
