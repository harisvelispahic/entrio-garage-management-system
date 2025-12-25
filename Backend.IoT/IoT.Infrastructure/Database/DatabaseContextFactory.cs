using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IoT.Infrastructure.Database;

public class DatabaseContextFactory
    : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        // 🔑 Resolve solution root reliably
        var basePath = AppContext.BaseDirectory;

        // Walk up until we find IoT.API
        while (!Directory.Exists(Path.Combine(basePath, "IoT.API")))
        {
            basePath = Directory.GetParent(basePath)!.FullName;
        }

        var apiPath = Path.Combine(basePath, "IoT.API");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new DatabaseContext(options);
    }
}
