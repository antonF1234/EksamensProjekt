using Npgsql;
using EksamensProjekt.Models;
using EksamensProjektAPI;

public class UserRepo
{
    private string Conn => DbConnectionInfo.ConnectionString; // We use the DbConnectionInfo class to get the connection string

    public async Task<UserModel?> GetByUsernameAsync(string username) // returns a user model if the username exists
    {
        await using var conn = new NpgsqlConnection(Conn); // set Postgre connection to conn
        await conn.OpenAsync(); // open connection to database

        await using var cmd = new NpgsqlCommand(
            "SELECT user_id, username, password, email, is_admin FROM users WHERE username = @u LIMIT 1",
            conn);
        cmd.Parameters.AddWithValue("u", username); // use username parameter from the function to get the user info

        await using var reader = await cmd.ExecuteReaderAsync(); // execute the command with async
    
        await reader.ReadAsync(); // read the response
        if (reader.HasRows == false) return null; // if there are no rows return null

        return new UserModel // insert the gotten user info into the new user model
        {
            UserId = reader.GetInt32(0),
            Username = reader.GetString(1),
            Password = reader.GetString(2),
            Email = reader.GetString(3),
            IsAdmin = reader.GetBoolean(4)
        };
    }



    public async Task<bool> InsertAsync(UserModel user) // inserts a new user into the databasem using InsertAsync from UserRepo
    {
        
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO users (username, password, email, is_admin) VALUES (@u, @p, @e, @a)",
            conn);
        cmd.Parameters.AddWithValue("u", user.Username); // use model username to insert into database
        cmd.Parameters.AddWithValue("p", user.Password);
        cmd.Parameters.AddWithValue("e", user.Email);
        cmd.Parameters.AddWithValue("a", user.IsAdmin);

        await cmd.ExecuteNonQueryAsync(); // execute the query
        return true; // if the query is executed successfully return true, this is not used for now....
    }


}