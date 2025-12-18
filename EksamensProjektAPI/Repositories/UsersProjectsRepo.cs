using Npgsql;
using EksamensProjektAPI;
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

// Repository-lag der håndterer relationen mellem brugere og projekter. (many-to-many relation via users_projects-tabellen).
public class UsersProjectsRepo
{
    private string Conn => DbConnectionInfo.ConnectionString; // Connection string til databasen

    public async Task<UsersProjectsModel> GetByUserId(int UserId) // Henter et projekt-tilknytningsobjekt baseret på brugerens ID. Retunerer UsersProjectsModel hvis relationen findes, ellers null
    {
        await using var conn = new NpgsqlConnection(Conn); // Opretter og åbner forbindelse til PostgreSQL
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand( // SQL-forespørgsel der finder projekter knyttet til en bruger
            "SELECT * FROM users_projects WHERE user_id = @uid",
            conn);
        cmd.Parameters.AddWithValue("uid", UserId);

        await using var reader = await cmd.ExecuteReaderAsync();  // Udfører forespørgslen
    
        await reader.ReadAsync();
        if (reader.HasRows == false) return null; // Hvis der ikke findes nogen relation returneres null

        return new UsersProjectsModel // Mapper databaserækken til UsersProjectsModel
        {
            UserProject = reader.GetInt32(0),
            UserId = reader.GetInt32(1),
            ProjectId = reader.GetInt32(2)
        };
    }
}