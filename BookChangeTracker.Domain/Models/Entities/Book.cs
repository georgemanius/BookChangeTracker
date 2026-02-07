using BookChangeTracker.Domain.Models.Events;

namespace BookChangeTracker.Domain.Models.Entities;

public class Book
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateOnly PublishDate { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = [];
    
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    
    public void AddAuthor(Author author)
    {
        if (BookAuthors.All(ba => ba.AuthorId != author.Id))
        {
            BookAuthors.Add(new BookAuthor
            {
                Book = this,
                Author = author
            });
            
            _domainEvents.Add(new AuthorAddedToBookEvent(Id, author.Id, author.Name, DateTime.UtcNow));
        }
    }
    
    public void RemoveAuthor(int authorId, string authorName)
    {
        BookAuthor? bookAuthor = BookAuthors.SingleOrDefault(ba => ba.AuthorId == authorId);
        if (bookAuthor is null)
        {
            return;
        }
        BookAuthors.Remove(bookAuthor);
        _domainEvents.Add(new AuthorRemovedFromBookEvent(Id, authorId, authorName, DateTime.UtcNow));
    }
    
    public void ClearDomainEvents() => _domainEvents.Clear();
}
