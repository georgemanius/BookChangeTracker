using BookChangeTracker.Api.Extensions;
using BookChangeTracker.Api.Models.Requests;
using BookChangeTracker.Api.Models.Responses;
using BookChangeTracker.Domain.Abstractions;
using BookChangeTracker.Domain.Models.Entities;
using BookChangeTracker.Domain.Models.Events;
using BookChangeTracker.Infrastructure;
using BookChangeTracker.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationDbContext(builder.Configuration)
    .AddDomainEventHandling()
    .AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Change Tracker API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

var eventPublisher = app.Services.GetRequiredService<IDomainEventPublisher>();
var eventDispatcher = app.Services.GetRequiredService<IDomainEventDispatcher>();

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
authorsGroup.MapGet("", async (ApplicationDbContext context) =>
{
    var authors = await context.Authors
        .Select(a => new AuthorResponse(a.Id, a.Name))
        .ToListAsync();
    return Results.Ok(authors);
})
.WithName("GetAuthors")
.Produces<List<AuthorResponse>>(200)
.WithDescription("Get all authors");

// POST /api/authors
authorsGroup.MapPost("", async (
    ApplicationDbContext context,
    CreateAuthorRequest request) =>
{
    var author = new Author { Name = request.Name };
    context.Authors.Add(author);
    await context.SaveChangesAsync();

    return Results.Created($"/api/authors/{author.Id}", new AuthorResponse(author.Id, author.Name));
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
booksGroup.MapGet("", async (ApplicationDbContext context) =>
{
    var books = await context.Books
        .Include(b => b.BookAuthors)
        .ThenInclude(ba => ba.Author)
        .Select(b => new BookResponse(
            b.Id,
            b.Title,
            b.Description,
            b.PublishDate,
            b.BookAuthors.Select(ba => new AuthorResponse(ba.Author.Id, ba.Author.Name)).ToList()))
        .ToListAsync();
    return Results.Ok(books);
})
.WithName("GetBooks")
.Produces<List<BookResponse>>(200)
.WithDescription("Get all books with their authors");

// GET /api/books/{id}
booksGroup.MapGet("{id}", async (
    int id,
    ApplicationDbContext context) =>
{
    var book = await context.Books
        .Include(b => b.BookAuthors)
        .ThenInclude(ba => ba.Author)
        .SingleOrDefaultAsync(b => b.Id == id);

    if (book is null)
        return Results.NotFound();

    var bookResponse = new BookResponse(
        book.Id,
        book.Title,
        book.Description,
        book.PublishDate,
        book.BookAuthors.Select(ba => new AuthorResponse(ba.Author.Id, ba.Author.Name)).ToList());

    return Results.Ok(bookResponse);
})
.WithName("GetBookById")
.Produces<BookResponse>(200)
.Produces(404)
.WithDescription("Get a specific book by ID");

// POST /api/books
booksGroup.MapPost("", async (
    ApplicationDbContext context,
    CreateBookRequest request) =>
{
    var book = new Book
    {
        Title = request.Title,
        Description = request.Description,
        PublishDate = request.PublishDate
    };

    context.Books.Add(book);
    await context.SaveChangesAsync();

    var bookResponse = new BookResponse(
        book.Id,
        book.Title,
        book.Description,
        book.PublishDate,
        []);

    return Results.Created($"/api/books/{book.Id}", bookResponse);
})
.WithName("CreateBook")
.Produces<BookResponse>(201)
.Produces(400)
.WithDescription("Create a new book");

// PUT /api/books/{id}
booksGroup.MapPut("{id}", async (
    int id,
    ApplicationDbContext context,
    UpdateBookRequest request) =>
{
    var book = await context.Books.FindAsync(id);
    if (book is null)
        return Results.NotFound();

    if (!string.IsNullOrEmpty(request.Title))
        book.Title = request.Title;

    if (!string.IsNullOrEmpty(request.Description))
        book.Description = request.Description;

    if (request.PublishDate.HasValue)
        book.PublishDate = request.PublishDate.Value;

    context.Books.Update(book);
    await context.SaveChangesAsync();

    var bookResponse = new BookResponse(
        book.Id,
        book.Title,
        book.Description,
        book.PublishDate,
        []);

    return Results.Ok(bookResponse);
})
.WithName("UpdateBook")
.Produces<BookResponse>(200)
.Produces(404)
.WithDescription("Update a book's properties");

// AUTHORS

var bookAuthorsGroup = booksGroup.MapGroup("{id}/authors")
    .WithTags("Book Authors")
    .WithOpenApi();

// POST /api/books/{id}/authors/{authorId}
bookAuthorsGroup.MapPost("{authorId}", async (
    int id,
    int authorId,
    ApplicationDbContext context) =>
{
    var book = await context.Books
        .Include(b => b.BookAuthors)
        .ThenInclude(bookAuthor => bookAuthor.Author)
        .SingleOrDefaultAsync(b => b.Id == id);
    if (book is null)
        return Results.NotFound("Book not found");

    var author = await context.Authors.SingleOrDefaultAsync(a => a.Id == authorId);
    if (author is null)
        return Results.NotFound("Author not found");

    if (book.BookAuthors.Any(ba => ba.AuthorId == authorId))
        return Results.BadRequest("Author is already assigned to this book");

    book.AddAuthor(author);
    await context.SaveChangesAsync();

    var bookResponse = new BookResponse(
        book.Id,
        book.Title,
        book.Description,
        book.PublishDate,
        book.BookAuthors.Select(ba => new AuthorResponse(ba.Author.Id, ba.Author.Name)).ToList());

    return Results.Ok(bookResponse);
})
.WithName("AddAuthorToBook")
.Produces<BookResponse>(200)
.Produces(404)
.Produces(400)
.WithDescription("Add an author to a book");

// DELETE /api/books/{id}/authors/{authorId}
bookAuthorsGroup.MapDelete("{authorId}", async (
    int id,
    int authorId,
    ApplicationDbContext context) =>
{
    var book = await context.Books
        .Include(b => b.BookAuthors)
        .ThenInclude(ba => ba.Author)
        .SingleOrDefaultAsync(b => b.Id == id);
    if (book is null)
        return Results.NotFound("Book not found");

    var bookAuthor = book.BookAuthors.FirstOrDefault(ba => ba.AuthorId == authorId);
    if (bookAuthor is null)
        return Results.NotFound("Author is not assigned to this book");

    var authorName = bookAuthor.Author.Name;

    book.RemoveAuthor(authorId, authorName);
    await context.SaveChangesAsync();

    var bookResponse = new BookResponse(
        book.Id,
        book.Title,
        book.Description,
        book.PublishDate,
        book.BookAuthors.Select(ba => new AuthorResponse(ba.Author.Id, ba.Author.Name)).ToList());

    return Results.Ok(bookResponse);
})
.WithName("RemoveAuthorFromBook")
.Produces<BookResponse>(200)
.Produces(404)
.WithDescription("Remove an author from a book");

// BOOK CHANGES

var changesGroup = booksGroup.MapGroup("{id}/changes")
    .WithTags("Change History")
    .WithOpenApi();

// GET /api/books/{id}/changes
changesGroup.MapGet("", async (
    int id,
    ApplicationDbContext context,
    [AsParameters] ChangeLogFilterRequest filter,
    [AsParameters] PaginationRequest pagination) =>
{
    var query = context.BookChangeLogs.AsQueryable();

    query = query
        .ApplyFiltering(id, filter)
        .ApplySorting(filter.OrderBy, filter.SortOrder);

    var totalCount = await query.CountAsync();

    var changes = await query
        .ApplyPagination(pagination)
        .Select(cl => new BookChangeLogResponse(
            cl.Id,
            cl.BookId,
            cl.FieldName,
            cl.OldValue,
            cl.NewValue,
            cl.ChangedAt,
            cl.Description,
            cl.ChangedBy,
            cl.ChangeType.ToString(),
            cl.AuthorId))
        .ToListAsync();

    var result = new PagedResult<BookChangeLogResponse>(
        changes,
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