using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookChangeTracker.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IChangeLogService, ChangeLogService>();

        return services;
    }
}
