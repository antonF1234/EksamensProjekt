using Npgsql;
using EksamensProjekt.Models;
using EksamensProjektAPI;

public class UserRepo
{
    private string Conn => DbConnectionInfo.ConnectionString; // Connection string hentes fra fælles DbConnectionInfo-klasse
    
    // Henter en bruger baseret på brugernavn. Retunerer UserModel hvis brugeren findes, ellers null.
    public async Task<UserModel?> GetByUsernameAsync(string username) 
    {
        await using var conn = new NpgsqlConnection(Conn); // Opretter og åbner forbindelse til PostgreSQL
        await conn.OpenAsync(); 

        // SQL-forespørgsel med parameter for at undgå SQL injection
        await using var cmd = new NpgsqlCommand(
            "SELECT user_id, username, password, email, is_admin FROM users WHERE username = @u LIMIT 1",
            conn);
        cmd.Parameters.AddWithValue("u", username); 

        await using var reader = await cmd.ExecuteReaderAsync(); // Udfører forespørgslen
    
        await reader.ReadAsync(); 
        if (reader.HasRows == false) return null; // Returnerer null hvis brugeren ikke findes

        return new UserModel // Mapper database-rækken til UserModel
        {
            UserId = reader.GetInt32(0),
            Username = reader.GetString(1),
            Password = reader.GetString(2),
            Email = reader.GetString(3),
            IsAdmin = reader.GetBoolean(4)
        };
    }



    public async Task<bool> InsertAsync(UserModel user) // Indsætter en ny bruger i databasen. Retunere True hvis indsættelsen lykkes.
    {
        
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // SQL INSERT-kommando
        await using var cmd = new NpgsqlCommand(
            "INSERT INTO users (username, password, email, is_admin) VALUES (@u, @p, @e, @a)",
            conn);
        cmd.Parameters.AddWithValue("u", user.Username); 
        cmd.Parameters.AddWithValue("p", user.Password);
        cmd.Parameters.AddWithValue("e", user.Email);
        cmd.Parameters.AddWithValue("a", user.IsAdmin);

        await cmd.ExecuteNonQueryAsync(); // Udfører kommandoen
        return true; // Returneres pt. ikke brugt, men klar til validering
    }

    public async Task<List<UserModel>> GetAllUsersAsync() // Henter alle brugere fra databasen. Password returneres ikke af sikkerhedsmæssige årsager. Retunerer liste af brugere
    {
        var users = new List<UserModel>();

        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // SQL-forespørgsel der henter alle brugere uden password
        await using var cmd = new NpgsqlCommand(
            "SELECT user_id, username, email, is_admin FROM users",
            conn);

        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync()) // Mapper hver række til UserModel
        {
            users.Add(new UserModel
            {
                UserId = reader.GetInt32(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                IsAdmin = reader.GetBoolean(3)
            });
        }

        return users;
    }

}