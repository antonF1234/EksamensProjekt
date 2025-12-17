using Npgsql;
using EksamensProjekt.Models;
using EksamensProjektAPI;

namespace EksamensProjektAPI.Repositories;

public class SkillRepo
{
    private string Conn => DbConnectionInfo.ConnectionString;

    public async Task<bool> InsertAsync(SkillModel skill) // Metode til at oprette en ny skill, tager en skillModel som parameter
    {
        await using var conn = new NpgsqlConnection(Conn); // bruger connection string fra DbConnectionInfo
        await conn.OpenAsync(); // åbner forbindelsen asyncront

        await using var cmd = new NpgsqlCommand(
            "INSERT INTO skills (name) VALUES (@n)",
            conn);
        cmd.Parameters.AddWithValue("n", skill.Name); // bruger skill.Name som værdi for parameter @n og indsætter den i SQL-kommandoen
        
        await cmd.ExecuteNonQueryAsync(); // Kør query
        return true; // Returner true, bliver ikke brugt i denne version
    }

    public async Task<List<SkillModel>> GetAllAsync() // Metode til at hente alle skills fra databasen
    {
        await using var conn = new NpgsqlConnection(Conn); // bruger connection string fra DbConnectionInfo
        await conn.OpenAsync(); // åbmer forbindelsen asyncront
        
        await using var cmd = new NpgsqlCommand(
            "SELECT * FROM skills", conn); // SQL-kommando til at hente alle skills, stjerne betyder at alle felter skal hentes
        
        await using var reader = await cmd.ExecuteReaderAsync(); // lav en variabel ved navnet reader som læser resultatet af SQL-kommandoen
        var skills = new List<SkillModel>(); // initialiserer en liste af SkillModel's

        while (await reader.ReadAsync()) // for hvert row i resultatet tilføj det til listen skills
        {
            skills.Add(new SkillModel // mapper hvert row til et SkillModel-objekt
            {
                SkillId = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }
        return skills; // returner listen med alle skills
    }
}
