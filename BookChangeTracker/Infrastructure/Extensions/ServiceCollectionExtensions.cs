using BookChangeTracker.Infrastructure.EventHandlers;
using BookChangeTracker.Models.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace BookChangeTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddDomainEventHandling(this IServiceCollection services)
    {
        services.AddSingleton<IDomainEventPublisher, DomainEventPublisher>();
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddScoped<IEventHandler<AuthorAddedToBookEvent>, AuthorAddedEventHandler>();
        services.AddScoped<IEventHandler<AuthorRemovedFromBookEvent>, AuthorRemovedEventHandler>();
        services.AddScoped<IEventHandler<BookPropertyChangedEvent>, BookPropertyChangedEventHandler>();

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
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
