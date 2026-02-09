using BookChangeTracker.Api.Extensions;
using BookChangeTracker.Api.Middleware;
using BookChangeTracker.Api.Models.Requests;
using BookChangeTracker.Api.Models.Responses;
using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Extensions;
using BookChangeTracker.Application.Models;
using BookChangeTracker.Domain.Abstractions;
using BookChangeTracker.Infrastructure;
using BookChangeTracker.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddSwagger();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseSwaggerUI();
app.UseHttpsRedirection();

var eventPublisher = app.Services.GetRequiredService<IEventPublisher>();
var eventDispatcher = app.Services.GetRequiredService<IEventDispatcher>();

eventPublisher.Subscribe<IDomainEvent>(eventDispatcher.DispatchAsync);

// TODO: implement a separate migration tool to run in CI/CD before deploying the main app
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}

// ENDPOINTS

var authorsGroup = app.MapGroup("/api/authors")
    .WithTags("Authors")
    .WithOpenApi();

// GET /api/authors
authorsGroup.MapGet("", async (IAuthorService authorService) =>
{
    var result = await authorService.GetAllAsync();
    
    return result switch
    {
        Result.SuccessResult<List<AuthorDto>> success => Results.Ok(success.Data.Select(a => a.ToResponse()).ToList()),
        _ => Results.StatusCode(500)
    };
})
.WithName("GetAuthors")
.Produces<List<AuthorResponse>>(200)
.WithDescription("Get all authors");

// POST /api/authors
authorsGroup.MapPost("", async (
    IAuthorService authorService,
    CreateAuthorRequest request) =>
{
    var result = await authorService.CreateAsync(request.ToCreateAuthorDto());
    
    return result switch
    {
        Result.SuccessResult<AuthorDto> success => Results.Created($"/api/authors/{success.Data.Id}", success.Data.ToResponse()),
        Result.FailureResult failure => Results.BadRequest(new ErrorResponse(failure.Error.Code, failure.Error.Message)),
        _ => Results.StatusCode(500)
    };
})
.WithName("CreateAuthor")
.Produces<AuthorResponse>(201)
.Produces<ErrorResponse>(400)
.WithDescription("Create a new author");

// DELETE /api/authors/{id}
authorsGroup.MapDelete("{id:int}", async (
    int id,
    IAuthorService authorService) =>
{
    var result = await authorService.DeleteAsync(id);
    
    return result is Result.SuccessResult
        ? Results.NoContent()
        : result is Result.FailureResult failure
            ? Results.NotFound(new ErrorResponse(failure.Error.Code, failure.Error.Message))
            : Results.StatusCode(500);
})
.WithName("DeleteAuthor")
.Produces(204)
.Produces<ErrorResponse>(404)
.WithDescription("Delete an author");

// BOOKS

var booksGroup = app.MapGroup("/api/books")
    .WithTags("Books")
    .WithOpenApi();

// GET /api/books
booksGroup.MapGet("", async (IBookService bookService) =>
{
    var result = await bookService.GetAllAsync();
    
    return result switch
    {
        Result.SuccessResult<List<BookDto>> success => Results.Ok(success.Data.Select(b => b.ToResponse()).ToList()),
        _ => Results.StatusCode(500)
    };
})
.WithName("GetBooks")
.Produces<List<BookResponse>>(200)
.WithDescription("Get all books with their authors");

// GET /api/books/{id}
booksGroup.MapGet("{id:int}", async (
    int id,
    IBookService bookService) =>
{
    var result = await bookService.GetByIdAsync(id);
    
    return result switch
    {
        Result.SuccessResult<BookDto> success => Results.Ok(success.Data.ToResponse()),
        Result.FailureResult failure => Results.NotFound(new ErrorResponse(failure.Error.Code, failure.Error.Message)),
        _ => Results.StatusCode(500)
    };
})
.WithName("GetBookById")
.Produces<BookResponse>(200)
.Produces<ErrorResponse>(404)
.WithDescription("Get a specific book by ID");

// POST /api/books
booksGroup.MapPost("", async (
    IBookService bookService,
    CreateBookRequest request) =>
{
    var result = await bookService.CreateAsync(request.ToCreateBookDto());
    
    return result switch
    {
        Result.SuccessResult<BookDto> success => Results.Created($"/api/books/{success.Data.Id}", success.Data.ToResponse()),
        Result.FailureResult failure => Results.BadRequest(new ErrorResponse(failure.Error.Code, failure.Error.Message)),
        _ => Results.StatusCode(500)
    };
})
.WithName("CreateBook")
.Produces<BookResponse>(201)
.Produces<ErrorResponse>(400)
.WithDescription("Create a new book");

// PUT /api/books/{id}
booksGroup.MapPut("{id:int}", async (
    int id,
    IBookService bookService,
    UpdateBookRequest request) =>
{
    var result = await bookService.UpdateAsync(id, request.ToUpdateBookDto());
    
    return result switch
    {
        Result.SuccessResult<BookDto> success => Results.Ok(success.Data.ToResponse()),
        Result.FailureResult failure => Results.NotFound(new ErrorResponse(failure.Error.Code, failure.Error.Message)),
        _ => Results.StatusCode(500)
    };
})
.WithName("UpdateBook")
.Produces<BookResponse>(200)
.Produces<ErrorResponse>(404)
.WithDescription("Update a book's properties");

// DELETE /api/books/{id}
booksGroup.MapDelete("{id:int}", async (
    int id,
    IBookService bookService) =>
{
    var result = await bookService.DeleteAsync(id);
    
    return result is Result.SuccessResult
        ? Results.NoContent()
        : result is Result.FailureResult failure
            ? Results.NotFound(new ErrorResponse(failure.Error.Code, failure.Error.Message))
            : Results.StatusCode(500);
})
.WithName("DeleteBook")
.Produces(204)
.Produces<ErrorResponse>(404)
.WithDescription("Delete a book");

// AUTHORS

var bookAuthorsGroup = booksGroup.MapGroup("{id:int}/authors")
    .WithTags("Book Authors")
    .WithOpenApi();

// POST /api/books/{id}/authors/{authorId}
bookAuthorsGroup.MapPost("{authorId:int}", async (
    int id,
    int authorId,
    IBookService bookService) =>
{
    var result = await bookService.AddAuthorAsync(id, authorId);
    
    return result switch
    {
        Result.SuccessResult<BookDto> success => Results.Ok(success.Data.ToResponse()),
        Result.FailureResult failure when failure.Error.Code == ErrorCodes.BookNotFound || failure.Error.Code == ErrorCodes.AuthorNotFound
            => Results.NotFound(new ErrorResponse(failure.Error.Code, failure.Error.Message)),
        Result.FailureResult failure when failure.Error.Code == ErrorCodes.AuthorAlreadyAssigned
            => Results.BadRequest(new ErrorResponse(failure.Error.Code, failure.Error.Message)),
        Result.FailureResult failure => Results.BadRequest(new ErrorResponse(failure.Error.Code, failure.Error.Message)),
        _ => Results.StatusCode(500)
    };
})
.WithName("AddAuthorToBook")
.Produces<BookResponse>(200)
.Produces<ErrorResponse>(404)
.Produces<ErrorResponse>(400)
.WithDescription("Add an author to a book");

// DELETE /api/books/{id}/authors/{authorId}
bookAuthorsGroup.MapDelete("{authorId:int}", async (
    int id,
    int authorId,
    IBookService bookService) =>
{
    var result = await bookService.RemoveAuthorAsync(id, authorId);
    
    return result switch
    {
        Result.SuccessResult<BookDto> success => Results.Ok(success.Data.ToResponse()),
        Result.FailureResult failure => Results.NotFound(new ErrorResponse(failure.Error.Code, failure.Error.Message)),
        _ => Results.StatusCode(500)
    };
})
.WithName("RemoveAuthorFromBook")
.Produces<BookResponse>(200)
.Produces<ErrorResponse>(404)
.WithDescription("Remove an author from a book");

// BOOK CHANGES

var changesGroup = booksGroup.MapGroup("{id:int}/changes")
    .WithTags("Change History")
    .WithOpenApi();

// GET /api/books/{id}/changes
changesGroup.MapGet("", async (
    int id,
    IChangeLogService changeLogService,
    [AsParameters] ChangeLogFilterRequest filter,
    [AsParameters] PaginationRequest pagination) =>
{
    var paginationDto = pagination.ToPaginationDto();
    var result = await changeLogService.GetBookChangeLogsAsync(
        id,
        filter.ToChangeLogFilterDto(),
        paginationDto);

    return result switch
    {
        Result.SuccessResult<(List<BookChangeLogDto> Logs, int TotalCount)> success =>
            Results.Ok(new PagedResult<BookChangeLogResponse>(
                success.Data.Logs.Select(l => l.ToResponse()).ToList(),
                success.Data.TotalCount,
                paginationDto.PageNumber,
                paginationDto.PageSize)),
        Result.FailureResult failure => Results.NotFound(new ErrorResponse(failure.Error.Code, failure.Error.Message)),
        _ => Results.StatusCode(500)
    };
})
.WithName("GetBookChanges")
.Produces<PagedResult<BookChangeLogResponse>>(200)
.Produces<ErrorResponse>(404)
.WithDescription("Get change history for a book with filtering, ordering, and pagination");

app.Run();