namespace MarketAPI.DAL;

using System.Data;
using Dapper;
using MarketAPI.Abstraction;
using MarketAPI.Database.Models;

public class StudentService : IStudentService
{
    private readonly IDbConnection _dbConnection;

    public StudentService(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task AddStudent(Student student)
    {
        string sql = """
            INSERT INTO student (Id, Name, Email, Phone) 
            VALUES (@Id, @Name, @Email, @Phone)
            """;
        await _dbConnection.ExecuteAsync(sql, student);
    }

    public async Task<Student?> GetStudent(string id)
    {
        string sql = """
            SELECT Id, Name, Email,Phone FROM student WHERE Id = @Id
            """;
        return await _dbConnection.QueryFirstOrDefaultAsync<Student>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Student>> GetStudents()
    {
        string sql = """
            SELECT Id, Name, Email, Phone FROM student
            """;
        return await _dbConnection.QueryAsync<Student>(sql);
    }

    public async Task UpdateStudent(Student student)
    {
        string sql = """
            UPDATE student SET Name = @Name, Email = @Email, Phone = @Phone WHERE Id = @Id
            """;
        await _dbConnection.ExecuteAsync(sql, student);
    }

    public async Task DeleteStudent(string id)
    {
        string sql = """
            DELETE FROM student WHERE Id = @Id
            """;
        await _dbConnection.ExecuteAsync(sql, new { Id = id });
    }
}