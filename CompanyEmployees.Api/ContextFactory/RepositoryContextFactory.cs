using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.Api.ContextFactory;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseMySql(configuration.GetConnectionString("sqlConnection"),
                new MySqlServerVersion(new Version(8, 0, 29)),
                b => b.MigrationsAssembly("CompanyEmployees.Api"));
        return new RepositoryContext(builder.Options);
    }
}