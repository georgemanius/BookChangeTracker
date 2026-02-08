using BookChangeTracker.Api.Extensions;
using BookChangeTracker.Api.Models.Requests;
using BookChangeTracker.Api.Models.Responses;
using BookChangeTracker.Application.Abstractions;
using BookChangeTracker.Application.Extensions;
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
    var authors = await authorService.GetAllAsync();
    return Results.Ok(authors.Select(a => a.ToResponse()).ToList());
})
.WithName("GetAuthors")
.Produces<List<AuthorResponse>>(200)
.WithDescription("Get all authors");

// POST /api/authors
authorsGroup.MapPost("", async (
    IAuthorService authorService,
    CreateAuthorRequest request) =>
{
    var author = await authorService.CreateAsync(request.ToCreateAuthorDto());
    return Results.Created($"/api/authors/{author.Id}", author.ToResponse());
})
.WithName("CreateAuthor")
.Produces<AuthorResponse>(201)
.Produces(400)
.WithDescription("Create a new author");

// BOOKS

var booksGroup = app.MapGroup("/api/books")
    .WithTags("Books")
    .WithOpenApi();

// GET /api/books
booksGroup.MapGet("", async (IBookService bookService) =>
{
    var books = await bookService.GetAllAsync();
    return Results.Ok(books.Select(b => b.ToResponse()).ToList());
})
.WithName("GetBooks")
.Produces<List<BookResponse>>(200)
.WithDescription("Get all books with their authors");

// GET /api/books/{id}
booksGroup.MapGet("{id:int}", async (
    int id,
    IBookService bookService) =>
{
    var book = await bookService.GetByIdAsync(id);
    
    if (book is null)
        return Results.NotFound();

    return Results.Ok(book.ToResponse());
})
.WithName("GetBookById")
.Produces<BookResponse>(200)
.Produces(404)
.WithDescription("Get a specific book by ID");

// POST /api/books
booksGroup.MapPost("", async (
    IBookService bookService,
    CreateBookRequest request) =>
{
    var book = await bookService.CreateAsync(request.ToCreateBookDto());
    return Results.Created($"/api/books/{book.Id}", book.ToResponse());
})
.WithName("CreateBook")
.Produces<BookResponse>(201)
.Produces(400)
.WithDescription("Create a new book");

// PUT /api/books/{id}
booksGroup.MapPut("{id:int}", async (
    int id,
    IBookService bookService,
    UpdateBookRequest request) =>
{
    var book = await bookService.UpdateAsync(id, request.ToUpdateBookDto());
    
    return book is null 
        ? Results.NotFound() 
        : Results.Ok(book.ToResponse());
})
.WithName("UpdateBook")
.Produces<BookResponse>(200)
.Produces(404)
.WithDescription("Update a book's properties");

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
    var book = await bookService.AddAuthorAsync(id, authorId);
    
    return book is null 
        ? Results.NotFound("Book not found, author not found, or author is already assigned to this book") 
        : Results.Ok(book.ToResponse());
})
.WithName("AddAuthorToBook")
.Produces<BookResponse>(200)
.Produces(404)
.Produces(400)
.WithDescription("Add an author to a book");

// DELETE /api/books/{id}/authors/{authorId}
bookAuthorsGroup.MapDelete("{authorId:int}", async (
    int id,
    int authorId,
    IBookService bookService) =>
{
    var book = await bookService.RemoveAuthorAsync(id, authorId);
    
    return book is null 
        ? Results.NotFound("Book not found or author is not assigned to this book") 
        : Results.Ok(book.ToResponse());
})
.WithName("RemoveAuthorFromBook")
.Produces<BookResponse>(200)
.Produces(404)
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
    var (logs, totalCount) = await changeLogService.GetBookChangeLogsAsync(
        id,
        filter.ToChangeLogFilterDto(),
        pagination.ToPaginationDto());

    var result = new PagedResult<BookChangeLogResponse>(
        logs.Select(l => l.ToResponse()).ToList(),
        totalCount,
        pagination.PageNumber,
        pagination.PageSize);

    return Results.Ok(result);
})
.WithName("GetBookChanges")
.Produces<PagedResult<BookChangeLogResponse>>(200)
.Produces(404)
.WithDescription("Get change history for a book with filtering, ordering, and pagination");

app.Run();