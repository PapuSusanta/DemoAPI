namespace MarketAPI.Abstraction;

using MarketAPI.Database.Models;

public interface IStudentService
{
    Task<IEnumerable<Student>> GetStudents();
    Task<Student?> GetStudent(string id);
    Task AddStudent(Student student);
    Task UpdateStudent(Student student);
    Task DeleteStudent(string id);
}