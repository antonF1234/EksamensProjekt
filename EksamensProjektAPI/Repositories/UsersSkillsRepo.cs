using EksamensProjekt.Models;
using Npgsql;
namespace EksamensProjektAPI.Repositories;

public class UsersSkillsRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;
    
    public async Task InsertAsync(int skillId, int userId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand(
            "INSERT INTO users_skills (employee_id, skill_id) VALUES (@employee_id, @skill_id)",
            conn);

        cmd.Parameters.AddWithValue("employee_id", userId);
        cmd.Parameters.AddWithValue("skill_id", skillId);
        
        await cmd.ExecuteNonQueryAsync();
    }
    
    public async Task<List<UsersSkillsModel>> GetAllUserSkillsAsync(int userId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            @"SELECT 
              us.employee_skill_id,
              us.employee_id,
              us.skill_id,
              s.name AS skill_name
          FROM users_skills us
          JOIN skills s ON us.skill_id = s.skill_id
          WHERE us.employee_id = @employee_id", conn);

        cmd.Parameters.AddWithValue("employee_id", userId);

        await using var reader = await cmd.ExecuteReaderAsync();

        var userSkills = new List<UsersSkillsModel>();

        while (await reader.ReadAsync())
        {
            userSkills.Add(new UsersSkillsModel
            {
                UserSkillId = reader.GetInt32(0),
                UserId      = reader.GetInt32(1),
                SkillId     = reader.GetInt32(2),
                SkillName   = reader.GetString(3)
            });
        }

        return userSkills;
    }


    public async Task DeleteAsync(int userSkillId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand(
            "DELETE FROM users_skills WHERE employee_skill_id = @employee_skill_id", conn);
        
        cmd.Parameters.AddWithValue("employee_skill_id", userSkillId);
        
        await cmd.ExecuteNonQueryAsync();
    }
}