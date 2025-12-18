using Npgsql;
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class AuthRepo
{
    private string Conn => DbConnectionInfo.ConnectionString; //henter connection string
    
    public async Task<UserModel?> GetByUsernameAsync(string username) //henter en bruger udfra brugernavnet
    {
        //åbner forbindelse til databasen - den lukkes med await using igen efter
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // SQL-kommando der finder brugeren med det givne brugernavn
        await using var cmd = new NpgsqlCommand(
            "SELECT user_id, username, password, email, is_admin FROM users WHERE username=@username LIMIT 1",
            conn);
        cmd.Parameters.AddWithValue("username", username); //tilføjer parameter

        await using var reader = await cmd.ExecuteReaderAsync(); // Kører kommandoen og får resultatet
        if (!await reader.ReadAsync()) return null; // Hvis der ikke findes nogen bruger med det brugernavn → returner null med det samme

        return new UserModel // Ellers laver vi et UserModel med data fra databasen
        {
            UserId = reader.GetInt32(0),
            Username = reader.GetString(1),
            Password = reader.GetString(2),
            Email = reader.GetString(3),
            IsAdmin = reader.GetBoolean(4)
        };
    }

    public async Task InsertAsync(UserModel user) // gemmer en ny bruger i databasen, når admin opretter ny konto
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        // INSERT-kommando der laver en ny række i users-tabellen
        await using var cmd = new NpgsqlCommand(
            "INSERT INTO users (username, password, email, is_admin) VALUES (@username, @password, @email, @is_admin)",
            conn);

        cmd.Parameters.AddWithValue("username", user.Username);
        cmd.Parameters.AddWithValue("password", user.Password);
        cmd.Parameters.AddWithValue("email", user.Email);
        cmd.Parameters.AddWithValue("is_admin", user.IsAdmin);
        
        await cmd.ExecuteNonQueryAsync(); // Kører selve INSERT – gemmer brugeren i databasen
    }
}