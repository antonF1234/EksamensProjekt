using Npgsql;                          // Den her skal vi have for at kunne snakke med PostgreSQL
using EksamensProjekt.Models;          
namespace EksamensProjektAPI.Services;

public class TaskRepo
{
    
    private string Conn => DbConnectionInfo.ConnectionString;

    // Denne metode henter alle opgaver fra databasen – den bliver brugt på Tasks-siden
    public async Task<List<TaskModel>> GetAllAsync()
    {
        // Åbner forbindelse til databasen – await using gør at den lukker selv bagefter
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // Her er SQL-kommandoen der henter alt fra tasks-tabellen
        await using var cmd = new NpgsqlCommand(
            "SELECT task_id, name, description, start_date, deadline, completion_date, status, project_id FROM tasks ORDER BY task_id", conn);
        
        // Kører kommandoen og får et "reader"-objekt tilbage med resultatet
        await using var reader = await cmd.ExecuteReaderAsync();

        // Laver en tom liste hvor jeg kan putte alle opgaverne ind
        var tasks = new List<TaskModel>();

        // Så længe der er flere rækker i resultatet, så kører loopet
        while (await reader.ReadAsync())
        {
            // For hver række laver jeg et nyt TaskModel og fylder det med data fra databasen
            // Kolonnerne kommer i den rækkefølge jeg har skrevet i SELECT
            tasks.Add(new TaskModel
            {
                TaskId         = reader.GetInt32(0),                                          // Første kolonne er task_id
                Name           = reader.GetString(1),                                         // Anden er navnet
                Description    = reader.IsDBNull(2) ? null : reader.GetString(2),       // Hvis beskrivelsen er tom i DB, så sæt null
                StartDate      = reader.IsDBNull(3) ? null : reader.GetDateTime(3),    // Samme med startdato – den må godt være tom
                Deadline       = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                CompletionDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                Status         = reader.IsDBNull(6) ? null : reader.GetString(6),
                ProjectId      = reader.GetInt32(7)                                         // Projekt-id skal altid være der
            });
        }
        // Til sidst sender jeg hele listen tilbage
        return tasks;
    }

    // Henter kun én opgave ud fra dens id – giver null hvis den ikke findes
    public async Task<TaskModel?> GetByIdAsync(int tid)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(
            "SELECT task_id, name, description, start_date, deadline, completion_date, status, project_id FROM tasks WHERE task_id = @id", conn);
        
        // Tilføjer parameter så vi ikke får SQL-injection
        cmd.Parameters.AddWithValue("id", tid);
        
        await using var reader = await cmd.ExecuteReaderAsync();

        // Hvis der ikke er nogen række, så returner null med det samme
        if (!await reader.ReadAsync()) return null;

        // Ellers laver vi et TaskModel ligesom ovenfor
        return new TaskModel
        {
            TaskId         = reader.GetInt32(0),
            Name           = reader.GetString(1),
            Description    = reader.IsDBNull(2) ? null : reader.GetString(2),
            StartDate      = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
            Deadline       = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
            CompletionDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
            Status         = reader.IsDBNull(6) ? null : reader.GetString(6),
            ProjectId      = reader.GetInt32(7)
        };
    }

    // Henter alle opgaver der hører til ét bestemt projekt
    public async Task<List<TaskModel>> GetByProjectIdAsync(int pid)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(
            "SELECT task_id, name, description, start_date, deadline, completion_date, status, project_id FROM tasks WHERE project_id = @pid ORDER BY task_id", conn);
        cmd.Parameters.AddWithValue("pid", pid);
        await using var reader = await cmd.ExecuteReaderAsync();

        var tasks = new List<TaskModel>();
        while (await reader.ReadAsync())
        {
            tasks.Add(new TaskModel
            {
                TaskId         = reader.GetInt32(0),
                Name           = reader.GetString(1),
                Description    = reader.IsDBNull(2) ? null : reader.GetString(2),
                StartDate      = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                Deadline       = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                CompletionDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                Status         = reader.IsDBNull(6) ? null : reader.GetString(6),
                ProjectId      = reader.GetInt32(7)
            });
        }
        return tasks;
    }

    // Gemmer en helt ny opgave i databasen
    public async Task InsertAsync(TaskModel task)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(
            @"INSERT INTO tasks (name, description, start_date, deadline, completion_date, status, project_id) 
              VALUES (@name, @description, @start_date, @deadline, @completion_date, @status, @project_id)", conn);

        // Fylder alle parametre – nogle felter må gerne være tomme
        cmd.Parameters.AddWithValue("name", task.Name);
        cmd.Parameters.AddWithValue("description", (object?)task.Description ?? DBNull.Value);     // Hvis null, send DBNull i stedet
        cmd.Parameters.AddWithValue("start_date", (object?)task.StartDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("deadline", (object?)task.Deadline ?? DBNull.Value);
        cmd.Parameters.AddWithValue("completion_date", (object?)task.CompletionDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("status", (object?)task.Status ?? "Ny");                       // Hvis ingen status, så sæt "Ny"
        cmd.Parameters.AddWithValue("project_id", task.ProjectId);

        // Kører selve INSERT-kommandoen
        await cmd.ExecuteNonQueryAsync();
    }

    // Opdaterer kun status på en opgave (fx når nogen trykker "Færdig")
    public async Task UpdateTaskStatusAsync(int taskId, string status)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(
            "UPDATE tasks SET status=@status WHERE task_id=@taskId", conn);
        cmd.Parameters.AddWithValue("taskId", taskId);
        cmd.Parameters.AddWithValue("status", status);
        await cmd.ExecuteNonQueryAsync();
    }

    // Slet opgavet
    public async Task DeleteTaskAsync(int taskId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(
            "DELETE FROM tasks WHERE task_id=@taskId", conn);
        cmd.Parameters.AddWithValue("taskId", taskId);
        await cmd.ExecuteNonQueryAsync();
    }

    // Opdaterer en eksisterende opgave med nye værdier fra edit-modalen
    public async Task UpdateAsync(TaskModel task)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        var sql = @" UPDATE tasks 
                     SET name=@name, 
                         description=@description, 
                         start_date=@startdate, 
                         deadline=@deadline, 
                         completion_date=@completiondate, 
                         status=@status, 
                         project_id=@projectid 
                     WHERE task_id=@tid";
        await using var cmd = new NpgsqlCommand(sql, conn);

        // Samme null-håndtering som når vi opretter
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