using Npgsql;
using EksamensProjekt.Models;
using EksamensProjektAPI;

namespace EksamensProjektAPI.Repositories;

public class SkillRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    public async Task<bool> InsertAsync(SkillModel skill)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO skills (name) VALUES (@n)",
            conn);
        cmd.Parameters.AddWithValue("n", skill.Name); 
        
        await cmd.ExecuteNonQueryAsync(); // execute the query
        return true; // if the query is executed successfully return true, this is not used for now....
    }

    public async Task<List<SkillModel>> GetAllAsync()
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand(
            "SELECT * FROM skills", conn);
        
        await using var reader = await cmd.ExecuteReaderAsync();
        var skills = new List<SkillModel>();

        while (await reader.ReadAsync())
        {
            skills.Add(new SkillModel
            {
                SkillId = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }
        return skills;
    }
}
