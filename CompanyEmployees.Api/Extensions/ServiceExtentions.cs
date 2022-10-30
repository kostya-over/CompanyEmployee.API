using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Contracts;
using Services;

namespace CompanyEmployees.Api.Extentions;

public static class ServiceExtentions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("X-Pagination");
            });
        });
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(dbContextOptions => dbContextOptions
            .UseMySql(configuration.GetConnectionString("sqlConnection"),
                new MySqlServerVersion(new Version(8, 0, 11)))
        );
    }
}