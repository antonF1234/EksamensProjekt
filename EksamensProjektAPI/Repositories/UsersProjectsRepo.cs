using Npgsql;
using EksamensProjektAPI;
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class UsersProjectsRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    public async Task<UsersProjectsModel> GetByUserId(int UserId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "SELECT * FROM users_projects WHERE user_id = @uid",
            conn);
        cmd.Parameters.AddWithValue("uid", UserId);

        await using var reader = await cmd.ExecuteReaderAsync();
    
        await reader.ReadAsync();
        if (reader.HasRows == false) return null;

        return new UsersProjectsModel
        {
            UserProject = reader.GetInt32(0),
            UserId = reader.GetInt32(1),
            ProjectId = reader.GetInt32(2)
        };
    }
}