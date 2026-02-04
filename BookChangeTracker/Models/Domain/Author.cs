namespace BookChangeTracker.Models.Domain;

public class Author
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public ICollection<BookAuthor> BookAuthors { get; set; } = [];
}