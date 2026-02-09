using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.EventHandlers;
using BookChangeTracker.Application.Services;
using BookChangeTracker.Domain.Abstractions;
using BookChangeTracker.Domain.Models.Events;
using Microsoft.Extensions.DependencyInjection;

namespace BookChangeTracker.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddServices()
            .AddEventHandlers();
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IChangeLogService, ChangeLogService>();

        return services;
    }
    
    private static IServiceCollection AddEventHandlers(this IServiceCollection services)
    {
        services.AddScoped<IEventHandler<AuthorAddedToBookEvent>, AuthorAddedEventHandler>();
        services.AddScoped<IEventHandler<AuthorRemovedFromBookEvent>, AuthorRemovedEventHandler>();
        services.AddScoped<IEventHandler<BookPropertyChangedEvent>, BookPropertyChangedEventHandler>();

        return services;
    }
}
