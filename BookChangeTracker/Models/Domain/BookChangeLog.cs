namespace BookChangeTracker.Models.Domain;

public class BookChangeLog
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string FieldName { get; set; } = string.Empty;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public DateTime ChangedAt { get; set; }

    public string Description { get; set; } = string.Empty;

    public string ChangedBy { get; set; } = string.Empty;

    public ChangeType ChangeType { get; set; }

    public int? AuthorId { get; set; }
}