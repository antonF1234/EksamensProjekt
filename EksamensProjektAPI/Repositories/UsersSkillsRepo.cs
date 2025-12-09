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