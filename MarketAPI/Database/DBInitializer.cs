using DbUp;

namespace MarketAPI.Database;

public static class DBInitializer
{
    public static void Initialize(string connectionString)
    {
        var upgrader =
            DeployChanges.To
                .SQLiteDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly())
                .WithTransaction()
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw new Exception("Database upgrade failed", result.Error);
        }
    }

}
