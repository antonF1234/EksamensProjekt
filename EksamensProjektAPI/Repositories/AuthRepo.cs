using Npgsql;
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class AuthRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    public async Task<UserModel?> GetByUsernameAsync(string username)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT user_id, username, password, email, is_admin FROM users WHERE username=@username LIMIT 1",
            conn);
        cmd.Parameters.AddWithValue("username", username);

        await using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new UserModel
        {
            UserId = reader.GetInt32(0),
            Username = reader.GetString(1),
            Password = reader.GetString(2),
            Email = reader.GetString(3),
            IsAdmin = reader.GetBoolean(4)
        };
    }

    public async Task InsertAsync(UserModel user)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO users (username, password, email, is_admin) VALUES (@username, @password, @email, @is_admin)",
            conn);

        cmd.Parameters.AddWithValue("username", user.Username);
        cmd.Parameters.AddWithValue("password", user.Password);
        cmd.Parameters.AddWithValue("email", user.Email);
        cmd.Parameters.AddWithValue("is_admin", user.IsAdmin);
        
        await cmd.ExecuteNonQueryAsync();
    }
}