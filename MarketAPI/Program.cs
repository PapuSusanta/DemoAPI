global using FluentValidation;
using System.Data;
using MarketAPI.Abstraction;
using MarketAPI.DAL;
using MarketAPI.Database;
using MarketAPI.Endpoints;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddProblemDetails();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    var conString = builder.Configuration.GetConnectionString("dbcon");
    builder.Services.AddScoped<IDbConnection>(_ => new SqliteConnection(conString));

    builder.Services.AddScoped<IStudentService, StudentService>();
    builder.Services.AddScoped<IUserService, UserService>();

}
var app = builder.Build();
{
    app.UseHttpsRedirection();

    var connectionString = app.Configuration.GetConnectionString("dbcon");
    DBInitializer.Initialize(connectionString!);

    app.MapGet("/status", () => Results.Ok(new { status = "API is up and running" }));

    app.MapStudentEndpoints();
    app.MapUserEndpoints();

    app.Run();
}