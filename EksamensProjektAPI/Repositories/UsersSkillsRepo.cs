using EksamensProjekt.Models;
using Npgsql;
namespace EksamensProjektAPI.Repositories;

public class UsersSkillsRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;
    
    public async Task InsertAsync(int skillId, int userId) // metode til at oprette en ny users_skills relation, tager skillId og userId som parameter
    {
        await using var conn = new NpgsqlConnection(Conn); // bruger connection string fra DbConnectionInfo
        await conn.OpenAsync(); // åbner forbindelsen asyncront
        
        await using var cmd = new NpgsqlCommand(
            "INSERT INTO users_skills (employee_id, skill_id) VALUES (@employee_id, @skill_id)",
            conn); // SQL-kommando til at oprette en ny relation, indsætter skillId og userId i users_skills tabellen

        cmd.Parameters.AddWithValue("employee_id", userId); // map emplyee_id til userId
        cmd.Parameters.AddWithValue("skill_id", skillId); // map skill_id til skillId
        
        await cmd.ExecuteNonQueryAsync(); // kører SQL-kommandoen
    }
    
    public async Task<List<UsersSkillsModel>> GetAllUserSkillsAsync(int userId) // metode til at hente alle users_skills relationer for en bruger
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

        cmd.Parameters.AddWithValue("employee_id", userId); // tager userId som parameter

        await using var reader = await cmd.ExecuteReaderAsync(); // lav reader variabel for at hente data fra databasen

        var userSkills = new List<UsersSkillsModel>(); // initialiserer en liste af UsersSkillsModel's

        while (await reader.ReadAsync()) // læs hvert row i resultatet
        {
            userSkills.Add(new UsersSkillsModel // mapper hvert row til et UsersSkillsModel objekt
            {
                UserSkillId = reader.GetInt32(0),
                UserId      = reader.GetInt32(1),
                SkillId     = reader.GetInt32(2),
                SkillName   = reader.GetString(3)
            });
        }

        return userSkills; // returner listen med alle modeler
    }


    public async Task DeleteAsync(int skillId, int userId) // metode til at slette en users_skills relation
    {
        await using var conn = new NpgsqlConnection(Conn);
        await conn.OpenAsync();
        
        await using var cmd = new NpgsqlCommand(
            "DELETE FROM users_skills WHERE skill_id = @skill_id AND employee_id = @userId", conn);
                    // fjern relation hvis skill_id og userId matcher
        cmd.Parameters.AddWithValue("skill_id", skillId); // bruger skillId som parameter
        cmd.Parameters.AddWithValue("userId", userId); // bruger userId som parameter
        
        await cmd.ExecuteNonQueryAsync(); // kører SQL-kommandoen
    }
}