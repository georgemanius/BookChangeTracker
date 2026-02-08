using Microsoft.OpenApi.Models;

namespace BookChangeTracker.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Book Change Tracker API",
                Version = "v1",
                Description = "API for tracking changes to book entities with pagination, filtering, and ordering"
            });
        });

        return services;
    }
}

public static class WebApplicationExtensions
{
    public static WebApplication UseSwaggerUI(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Change Tracker API v1");
                options.RoutePrefix = "swagger";
            });
        }

        return app;
    }
}
