using Contracts;
using LoggerService;

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
                    .AllowAnyMethod();
            });
        });
    }

    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }
}