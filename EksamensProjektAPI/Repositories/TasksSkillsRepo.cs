using Npgsql;
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Repositories;

public class TasksSkillsRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    public async Task InsertTaskSkillAsync(int taskId, int skillId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand("INSERT INTO tasks_skills (task_id, skill_id) VALUES (@taskId, @skillId)", conn);
        
        cmd.Parameters.AddWithValue("taskId", taskId);
        cmd.Parameters.AddWithValue("skillId", skillId);
        
        await cmd.ExecuteNonQueryAsync();
    }
    
    public async Task DeleteTaskSkillAsync(int taskId, int skillId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand("DELETE FROM tasks_skills WHERE task_skill_id=@taskSkillId AND skill_id=@skillId", conn);
        
        cmd.Parameters.AddWithValue("taskSkillId", taskId);
        cmd.Parameters.AddWithValue("skillId", skillId);
        
        await cmd.ExecuteNonQueryAsync();
    }
    
    public async Task<List<SkillModel>> GetSkillsForTaskAsync(int taskId)
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();

        const string sql = @"
          SELECT s.*
           FROM skills s
          JOIN (
              SELECT skill_id
              FROM tasks_skills
              WHERE task_id = @taskId
              ) ts ON s.skill_id = ts.skill_id;
        ";


        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("taskId", taskId);

        var skills = new List<SkillModel>();

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            skills.Add(new SkillModel()
            {
                SkillId = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }
        return skills;
    }
}