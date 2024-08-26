namespace MarketAPI.Endpoints;

using MarketAPI.Abstraction;
using MarketAPI.Database.Models;
using MarketAPI.Filters;
using MarketAPI.Models.Student;
using Microsoft.AspNetCore.Http.HttpResults;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
    {
        var studentGroup = app.MapGroup("/student");

        studentGroup.MapGet("/", GetStudents);

        studentGroup.MapGet("/{id}", GetStudentById);

        studentGroup.MapPost("/", CreateStudent)
        .AddEndpointFilter<ValidationFilter<StudentRequest>>();

        studentGroup.MapPut("/{id}", UpdateStudent)
            .AddEndpointFilter<ValidationFilter<StudentRequest>>();
    }

    static async Task<Results<Ok<List<StudentResponse>>, ProblemHttpResult>> GetStudents(IStudentService studentService)
    {
        var student = await studentService.GetStudents();
        var response = new List<StudentResponse>();
        if (student == null)
        {
            return TypedResults.Ok(response);
        }
        response = student
                    .Select(s =>
                            new StudentResponse(s.Id, s.Name, s.Email, s.Phone))
                    .ToList();
        return TypedResults.Ok(response);
    }
    static async Task<Results<Ok<StudentResponse>, ProblemHttpResult>> CreateStudent(IStudentService studentService, StudentRequest request)
    {
        var Id = Guid.NewGuid().ToString();
        var student = new Student(Id, request.Name, request.Email, request.Phone);
        await studentService.AddStudent(student);

        var response = new StudentResponse(student.Id, student.Name, student.Email, student.Phone);

        return TypedResults.Ok(response);
    }
    static async Task<Results<NoContent, ProblemHttpResult>> UpdateStudent(IStudentService studentService, string id, StudentRequest request)
    {
        if (!Guid.TryParse(id, out Guid Id))
        {
            return TypedResults.Problem(
                title: "Bad Request",
                detail: "Invalid id",
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        var student = await studentService.GetStudent(Id.ToString());
        if (student == null)
        {
            return TypedResults.Problem(
                detail: "Bad Request",
                statusCode: StatusCodes.Status400BadRequest
            );
        }
        student = new Student(Id.ToString(), request.Name, request.Email, request.Phone);
        await studentService.UpdateStudent(student);
        return TypedResults.NoContent();
    }
    static async Task<Results<Ok<StudentResponse>, NotFound, ProblemHttpResult>> GetStudentById(IStudentService studentService, string id)
    {
        if (!Guid.TryParse(id.ToString(), out Guid Id))
        {
            return TypedResults.Problem(
                detail: "Invalid id",
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        var student = await studentService.GetStudent(Id.ToString());
        if (student == null)
        {
            return TypedResults.NotFound();
        }

        var response = new StudentResponse(student.Id, student.Name, student.Email, student.Phone);

        return TypedResults.Ok(response);
    }
}