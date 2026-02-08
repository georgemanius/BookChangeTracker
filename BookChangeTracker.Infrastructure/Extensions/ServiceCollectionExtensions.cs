using BookChangeTracker.Domain.Abstractions;
using BookChangeTracker.Domain.Models.Events;
using BookChangeTracker.Infrastructure.Abstractions;
using BookChangeTracker.Infrastructure.EventHandling.Handlers;
using BookChangeTracker.Infrastructure.EventHandling.Services;
using BookChangeTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookChangeTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplicationDbContext(configuration);
        services.AddDomainEventHandling();
        services.AddRepositories();

        return services;
    }

    public static IServiceCollection AddApplicationDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddDomainEventHandling(this IServiceCollection services)
    {
        services.AddSingleton<IEventPublisher, EventPublisher>();
        services.AddSingleton<IEventDispatcher, EventDispatcher>();

        services.AddScoped<IEventHandler<AuthorAddedToBookEvent>, AuthorAddedEventHandler>();
        services.AddScoped<IEventHandler<AuthorRemovedFromBookEvent>, AuthorRemovedEventHandler>();
        services.AddScoped<IEventHandler<BookPropertyChangedEvent>, BookPropertyChangedEventHandler>();

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IChangeLogRepository, ChangeLogRepository>();

        return services;
    }
}
