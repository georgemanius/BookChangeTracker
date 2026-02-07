using BookChangeTracker.Domain.Abstractions;
using BookChangeTracker.Domain.Models.Events;
using BookChangeTracker.Infrastructure.EventHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace BookChangeTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainEventHandling(this IServiceCollection services)
    {
        services.AddSingleton<IDomainEventPublisher, DomainEventPublisher>();
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddScoped<IEventHandler<AuthorAddedToBookEvent>, AuthorAddedEventHandler>();
        services.AddScoped<IEventHandler<AuthorRemovedFromBookEvent>, AuthorRemovedEventHandler>();
        services.AddScoped<IEventHandler<BookPropertyChangedEvent>, BookPropertyChangedEventHandler>();

        return services;
    }
}
