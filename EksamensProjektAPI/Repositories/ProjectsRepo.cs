using Npgsql;                    
using EksamensProjekt.Models;    
using EksamensProjektAPI;        

namespace EksamensProjektAPI.Services;

// Repository-klassen til alt, der har med projekter at gøre
public class ProjectRepo
{
    // Privat property – henter connection string fra vores centrale sted
    // På den måde skal vi kun ændre den ét sted, hvis den ændrer sig
    private string Conn => DbConnectionInfo.ConnectionString;

    // Metode: Hent ALLE projekter fra databasen
    public async Task<List<ProjectModel>> GetAllAsync()
    {
        // Opretter forbindelse – await using = lukker automatisk når vi er færdige
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();                                 // Åbner forbindelsen

        // SQL-kommando: Henter alt fra projects-tabellen, sorteret efter ID
        await using var cmd = new NpgsqlCommand(
            "SELECT project_id, name, description, deadline, status FROM projects ORDER BY project_id", conn);

        // Udfører SELECT-kommandoen og får en reader tilbage
        await using var reader = await cmd.ExecuteReaderAsync();

        // Opretter en tom liste, vi fylder projekter i
        var projects = new List<ProjectModel>();

        // Løber igennem ALLE rækker, databasen returnerer (kan være 0, 1 eller 100)
        while (await reader.ReadAsync())
        {
            projects.Add(new ProjectModel
            {
                ProjectId   = reader.GetInt32(0),                                   // Kolonne 0 = project_id
                Name        = reader.GetString(1),                                  // Kolonne 1 = name
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),     // Kolonne 2 kan være NULL
                Deadline    = reader.IsDBNull(3) ? null : reader.GetDateTime(3),   // Kolonne 3 kan være NULL
                Status      = reader.IsDBNull(4) ? null : reader.GetString(4)      // Kolonne 4 kan være NULL
            });
        }

        // Returnerer listen – tom hvis der ingen projekter er (aldrig null!)
        return projects;
    }

    // Metode: Hent ÉT specifikt projekt ud fra projekt-ID
    public async Task<ProjectModel?> GetByIdAsync(int id)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // SQL med parameter @id – beskytter mod SQL-injection
        await using var cmd = new NpgsqlCommand(
            "SELECT project_id, name, description, deadline, status FROM projects WHERE project_id = @id LIMIT 1", conn);
        cmd.Parameters.AddWithValue("id", id);    // Sikker parameter

        await using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();                 // Flytter læsepegeren til første (og eneste) række

        // Hvis der ikke findes et projekt med det ID → returner null
        if (!reader.HasRows) return null;

        // Ellers returner projektet som et ProjectModel-objekt
        return new ProjectModel
        {
            ProjectId   = reader.GetInt32(0),
            Name        = reader.GetString(1),
            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
            Deadline    = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
            Status      = reader.IsDBNull(4) ? null : reader.GetString(4)
        };
    }

    // Metode: Opret et helt nyt projekt i databasen
    public async Task InsertAsync(ProjectModel project)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // SQL INSERT-kommando – status sættes automatisk til 'Ny'
        await using var cmd = new NpgsqlCommand(
            @"INSERT INTO projects (name, description, deadline, status)
              VALUES (@name, @desc, @deadline, 'Ny')", conn);
    }
}