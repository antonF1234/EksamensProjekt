using Npgsql;
using BCrypt.Net;
using EksamensProjekt.Models;
using EksamensProjektAPI;

public class UserRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    public async Task<UserModel?> GetByUsernameAsync(string username)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT user_id, username, password, email, is_admin FROM users WHERE username = @u LIMIT 1",
            conn);
        cmd.Parameters.AddWithValue("u", username);

        await using var reader = await cmd.ExecuteReaderAsync();
    
        await reader.ReadAsync();
        if (reader.HasRows == false) return null;

        return new UserModel
        {
            UserId = reader.GetInt32(0),
            Username = reader.GetString(1),
            Password = reader.GetString(2),
            Email = reader.GetString(3),
            IsAdmin = reader.GetBoolean(4)
        };
    }



    public async Task<bool> InsertAsync(UserModel user)
    {
        
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO users (username, password, email, is_admin) VALUES (@u, @p, @e, @a)",
            conn);
        cmd.Parameters.AddWithValue("u", user.Username);
        cmd.Parameters.AddWithValue("p", user.Password);
        cmd.Parameters.AddWithValue("e", user.Email);
        cmd.Parameters.AddWithValue("a", user.IsAdmin);

        await cmd.ExecuteNonQueryAsync();
        return true;
    }


}